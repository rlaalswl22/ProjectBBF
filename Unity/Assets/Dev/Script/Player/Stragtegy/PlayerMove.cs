using System;
using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour, IPlayerStrategy
{
    private InputAction _movementAction;
    private InputAction _sprintAction;
    private PlayerController _controller;
    private Rigidbody2D _rigidbody;
    private PlayerMovementData _movementData;
    private PlayerBlackboard _blackboard;

    public Vector2 LastMovedDirection { get; set; }
    
    public void Init(PlayerController controller)
    {
        _movementData = controller.MovementData;
        _rigidbody = controller.Rigidbody;
        _controller = controller;
        _blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");
        
        BindInputAction();

        StartCoroutine(CoSteminaUpdate());
    }

    private void BindInputAction()
    {
        _movementAction = InputManager.Actions.Movement;
        _sprintAction = InputManager.Actions.Sprint;
    }

    private IEnumerator CoSteminaUpdate()
    {
        bool beforeSprint = false;
        while (true)
        {
            if (TimeManager.Instance.IsRunning is false)
            {
                yield return null;
                continue;
            }

            if (_sprintAction.IsPressed() is false && beforeSprint is false && _blackboard.Energy > 0)
            {
                _blackboard.Stemina += Time.deltaTime * _movementData.SteminaIncreasePerSec;
                yield return null;
                continue;
            }
            
            if (_sprintAction.IsPressed() && _blackboard.Stemina > 0f)
            {
                _blackboard.Stemina -= Time.deltaTime * _movementData.SteminaDecreasePerSec;
                beforeSprint = true;
                yield return null;
                continue;
            }

            float waitTimer = 0f;
            while (_sprintAction.IsPressed() is false && waitTimer < _movementData.SteminaIncreaseWaitDuration)
            {
                waitTimer += Time.deltaTime;
                yield return null;
            }

            beforeSprint = false;
            yield return null;
        }
    }
    
    public void OnMove()
    {
        var input = _movementAction.ReadValue<Vector2>();
        
        Vector2 dir = new Vector2(
            input.x,
            input.y
        );


        dir = dir.normalized;
        Vector2 velDir = Vector2.zero;
        
        if (_sprintAction.IsPressed() && _blackboard.Stemina > 0f)
        {
            velDir = dir * _movementData.SprintSpeed;
        }
        else
        {
            velDir = dir * _movementData.MovementSpeed;
        }

        _rigidbody.velocity = velDir;
        
        if (Mathf.Approximately(Mathf.Abs(input.x) + Mathf.Abs(input.y), 0f) == false)
        {
            LastMovedDirection = input.normalized;
            ChangeClip(dir, AnimationData.Movement.Walk);
        }
        else
        {
            ChangeClip(LastMovedDirection, AnimationData.Movement.Idle);
        }
        
    }

    private int GetCloserFactor(float x)
    {
        float j = x / (1f / 16f);

        float up = Mathf.Abs(Mathf.Ceil(1/j) - 1/j);
        float down = Mathf.Abs(Mathf.Floor(1/j) - 1/j);

        if (up < down)
        {
            return Mathf.RoundToInt(1 / up);
        }
        else
        {
            return Mathf.RoundToInt(1 / down);
        }
    }

    private void ChangeClip(Vector2 dir, AnimationData.Movement movementType)
    {
        var aniData = _controller.AnimationData;
        var visual = _controller.VisualStrategy;

        AnimationClip clip = null;
        
        // 왼쪽 위
        if (dir is { x: < 0, y: > 0 })
        {
            clip = aniData.GetClip(movementType, AnimationData.Direction.LeftUp);
        }
        // 오른쪽 위
        else if (dir is { x: > 0, y: > 0 })
        {
            clip = aniData.GetClip(movementType, AnimationData.Direction.RightUp);
        }
        // 왼쪽 아래
        else if (dir is { x: < 0, y: < 0 })
        {
            clip = aniData.GetClip(movementType, AnimationData.Direction.Left);
        }
        // 오른쪽 아래
        else if (dir is { x: > 0, y: < 0 })
        {
            clip = aniData.GetClip(movementType, AnimationData.Direction.Right);
        }
        
        // 위
        else if (dir is { x:  0, y: > 0 })
        {
            clip = aniData.GetClip(movementType, AnimationData.Direction.Up);
        }
        // 아래
        else if (dir is { x:  0, y: < 0 })
        {
            clip = aniData.GetClip(movementType, AnimationData.Direction.Down);
        }
        // 왼쪽
        else if (dir is { x: < 0, y:  0 })
        {
            clip = aniData.GetClip(movementType, AnimationData.Direction.Left);
        }
        // 오른쪽
        else if (dir is { x: > 0, y: 0 })
        {
            clip = aniData.GetClip(movementType, AnimationData.Direction.RightUp);
        }
        
        visual.ChangeClip(clip);
    }

    public void ResetVelocity()
    {
        _rigidbody.velocity = Vector3.zero;
        
        
        var aniData = _controller.AnimationData;
        var visual = _controller.VisualStrategy;

        ChangeClip(LastMovedDirection.normalized, AnimationData.Movement.Idle);
    }
}
