using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class PlayerVisual : MonoBehaviour, IPlayerStrategy
{
    private Animator _animator;

    private const string PLAYER_ANI_STATE = "DefaultMovement";

    private AnimationClip _defaultClip = null;
    private AnimationClip _beforeClip = null;

    public void Init(PlayerController controller)
    {
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
}