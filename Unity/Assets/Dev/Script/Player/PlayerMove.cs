using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour, IPlayerStrategy
{
    private InputAction _movementAction;
    private PlayerController _controller;
    private Rigidbody2D _rigidbody;
    private PlayerMovementData _movementData;

    public Vector2 LastMovedDirection { get; set; }
    
    public void Init(PlayerController controller)
    {
        _movementData = controller.MovementData;
        _rigidbody = controller.Rigidbody;
        _controller = controller;
        
        BindInputAction();
    }

    private void BindInputAction()
    {
        _movementAction = InputManager.Actions.Movement;
    }
    
    public void OnMove()
    {
        var input = _movementAction.ReadValue<Vector2>();
        
        Vector2 dir = new Vector2(
            input.x,
            input.y
        );


        dir = dir.normalized;
        _rigidbody.velocity = dir * _movementData.MovementSpeed;
        
        if (Mathf.Approximately(Mathf.Abs(input.x) + Mathf.Abs(input.y), 0f) == false)
        {
            LastMovedDirection = input.normalized;
            ChangeClip(dir, false);
        }
        else
        {
            ChangeClip(LastMovedDirection, true);
        }
        
    }

    private void ChangeClip(Vector2 dir, bool isIdle)
    {
        var aniData = _controller.AnimationData;
        var visual = _controller.VisualStrategy;

        // 왼쪽 위
        if (dir is { x: < 0, y: > 0 })
        {
            if (isIdle)
            {
                visual.ChangeClip(aniData.IdleLeftUp);
            }
            else
            {
                visual.ChangeClip(aniData.MovementLeftUp);
            }
        }
        // 오른쪽 위
        else if (dir is { x: > 0, y: > 0 })
        {
            if (isIdle)
            {
                visual.ChangeClip(aniData.IdleRightUp);
            }
            else
            {
                visual.ChangeClip(aniData.MovementRightUp);
            }
        }
        // 왼쪽 아래
        else if (dir is { x: < 0, y: < 0 })
        {
            if (isIdle)
            {
                visual.ChangeClip(aniData.IdleLeft);
            }
            else
            {
                visual.ChangeClip(aniData.MovementLeft);
            }
        }
        // 오른쪽 아래
        else if (dir is { x: > 0, y: < 0 })
        {
            if (isIdle)
            {
                visual.ChangeClip(aniData.IdleRight);
            }
            else
            {
                visual.ChangeClip(aniData.MovementRight);
            }
        }
        
        // 위
        else if (dir is { x:  0, y: > 0 })
        {
            if (isIdle)
            {
                visual.ChangeClip(aniData.IdleUp);
            }
            else
            {
                visual.ChangeClip(aniData.MovementUp);
            }
        }
        // 아래
        else if (dir is { x:  0, y: < 0 })
        {
            if (isIdle)
            {
                visual.ChangeClip(aniData.IdleDown);
            }
            else
            {
                visual.ChangeClip(aniData.MovementDown);
            }
        }
        // 왼쪽
        else if (dir is { x: < 0, y:  0 })
        {
            if (isIdle)
            {
                visual.ChangeClip(aniData.IdleLeft);
            }
            else
            {
                visual.ChangeClip(aniData.MovementLeft);
            }
        }
        // 오른쪽
        else if (dir is { x: > 0, y: 0 })
        {
            if (isIdle)
            {
                visual.ChangeClip(aniData.IdleRight);
            }
            else
            {
                visual.ChangeClip(aniData.MovementRight);
            }
        }
    }

    public void ResetVelocity()
    {
        _rigidbody.velocity = Vector3.zero;
        
        
        var aniData = _controller.AnimationData;
        var visual = _controller.VisualStrategy;

        ChangeClip(LastMovedDirection.normalized, true);
    }
}
