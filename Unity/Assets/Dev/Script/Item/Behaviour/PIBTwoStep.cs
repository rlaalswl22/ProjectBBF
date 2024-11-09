using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using UnityEngine;
public abstract class PIBTwoStep : PlayerItemBehaviour
{
    public enum ActionResult
    {
        Continue,
        Break,
        BreakWithoutUnlock,
    }
    
    [SerializeField] private float _durationForWaitTiming;
    [SerializeField] private float _durationForWaitTotal;

    public float WaitStep0 => _durationForWaitTiming;

    public float WaitStep1
    {
        get
        {
            float endDuration = _durationForWaitTotal - _durationForWaitTiming;
            endDuration = Mathf.Max(endDuration, 0f);
            
            return endDuration;
        }
    }

    public virtual bool UnlockMove => true;
    public virtual bool UnlockInteraction => true;
    public virtual bool LockMove => true;
    public virtual bool LockInteraction => true;

    protected abstract UniTask<ActionResult> PreAction(PlayerController playerController, ItemData itemData, CancellationToken token = default);
    protected abstract UniTask<ActionResult> PostAction(PlayerController playerController, ItemData itemData, CancellationToken token = default);

    public sealed override async UniTask DoAction(PlayerController playerController, ItemData itemData, CancellationToken token = default)
    {
        try
        {
            if (LockMove)
            {
                playerController.Blackboard.IsMoveStopped = true;
            }
            if (LockInteraction)
            {
                playerController.Blackboard.IsInteractionStopped = true;
            }

            ActionResult result = await PreAction(playerController, itemData, token);
            if (result == ActionResult.Break)
            {
                if (UnlockMove)
                {
                    playerController.Blackboard.IsMoveStopped = false;
                }
                if (UnlockInteraction)
                {
                    playerController.Blackboard.IsInteractionStopped = false;
                }
                return;
            }
            if (result == ActionResult.BreakWithoutUnlock) return;

            await playerController.Interactor.WaitForSecondAsync(WaitStep0, token);

            result = await PostAction(playerController, itemData, token);
            if (result == ActionResult.Break)
            {
                if (UnlockMove)
                {
                    playerController.Blackboard.IsMoveStopped = false;
                }
                if (UnlockInteraction)
                {
                    playerController.Blackboard.IsInteractionStopped = false;
                }
                return;
            }
            if (result == ActionResult.BreakWithoutUnlock) return;

            await playerController.Interactor.WaitForSecondAsync(WaitStep1, token);
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            Debug.LogException(e);
        }
    }

}