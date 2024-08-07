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

    private const string PLAYER_ANI_STATE = "DefaultMovement";

    private AnimationClip _defaultClip = null;
    private AnimationClip _beforeClip = null;

    public void Init(PlayerController controller)
    {
        _rigidbody = controller.Rigidbody;
        _animator = controller.Animator;
        
        RuntimeAnimatorController ac = _animator.runtimeAnimatorController;
        AnimatorOverrideController overrideController = new AnimatorOverrideController(ac);
        _animator.runtimeAnimatorController = overrideController;

        _defaultClip = overrideController[PLAYER_ANI_STATE];
    }
    
    public void ChangeClip(AnimationClip newClip, bool force = false)
    {
        if (force == false && _beforeClip == newClip) return;
        
        if (_animator.runtimeAnimatorController is AnimatorOverrideController overrideController)
        {
            overrideController[_defaultClip] = newClip;
            _beforeClip = newClip;
        }
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

        //_animator.transform.localScale = scale;
    }
}