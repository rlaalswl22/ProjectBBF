using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour, IPlayerStrategy
{
    private InputAction _movementAction;
    
    private Rigidbody2D _rigidbody;
    private PlayerMovementData _movementData;
    private Animator _animator;
    private static readonly int HashMovement = Animator.StringToHash("F_Movement");

    public Vector2 LastMovedDirection { get; set; }
    
    public void Init(PlayerController controller)
    {
        _movementData = controller.MovementData;
        _rigidbody = controller.Rigidbody;
        _animator = controller.Animator;
        
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


        dir *= _movementData.MovementSpeed;

        _rigidbody.velocity = dir;

        _animator.SetFloat(HashMovement, dir.magnitude);
        
        
        if (Mathf.Approximately(Mathf.Abs(input.x) + Mathf.Abs(input.y), 0f) == false)
        {
            LastMovedDirection = input.normalized;
        }
        
    }

    public void ResetVelocity()
    {
        _rigidbody.velocity = Vector3.zero;

        _animator.SetFloat(HashMovement, 0f);
    }
}
