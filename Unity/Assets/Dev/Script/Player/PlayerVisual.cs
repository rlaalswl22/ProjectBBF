using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class PlayerVisual : MonoBehaviour, IPlayerStrategy
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private static readonly int State = Animator.StringToHash("State");

    private bool _isLeft = false;

    public void Init(PlayerController controller)
    {
        _rigidbody = controller.Rigidbody;
        _animator = controller.Animator;
    }
    
    public void UpdateFlip()
    {
        if (Mathf.Approximately(_rigidbody.velocity.x, 0f))
        {
            return;
        }
        
        var velocity = _rigidbody.velocity;
        _isLeft = velocity.x < 0f;
        var scale = _animator.transform.lossyScale;
        scale.x = Mathf.Abs(scale.x);

        if (_isLeft == false)
        {
            scale.x *= -1f;
        }

        _animator.transform.localScale = scale;
    }
}