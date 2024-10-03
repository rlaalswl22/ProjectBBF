using System;
using System.Collections;
using System.Linq;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BakeryPressed: BakeryFlowBehaviourBucket, IObjectBehaviour
{
    [Serializable]
    public enum Resolvor
    {
        Dough,
        Additive,
        Baking,
    }

    [SerializeField] private Resolvor _resolvor;
    [SerializeField] private BakeryPressedData _data;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Transform _playPoint;

    private void Start()
    {
        _panel.SetActive(false);
    }

    protected override void OnActivate(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
        if (activator.Owner is not PlayerController pc) return;
        if (IsFullBucket is false) return;

        StartCoroutine(CoUpdate(pc));
    }

    protected override void OnEnter(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
    }

    protected override void OnExit(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
        StopAllCoroutines();
    }

    private (ItemData failItem, ItemData resultItem, float duration) GetResolvedItem()
    {
        var resolver = BakeryRecipeResolver.Instance;
        ItemData failItem;
        ItemData resultItem = null;
        float duration = 0f;
        var bucketItems = StoredItems;

        if (bucketItems.Count is 0)
        {
            Debug.LogError("Bucket의 수가 0입니다.");
            return (resolver.FailDoughRecipe.DoughItem, resolver.FailDoughRecipe.DoughItem, 0f);
        }

        switch (_resolvor)
        {
            case Resolvor.Dough:
                failItem = resolver.FailDoughRecipe.DoughItem;
                duration = resolver.FailDoughRecipe.KneadDuration;
                var doughRecipe = resolver.ResolveDough(bucketItems);

                if (doughRecipe)
                {
                    resultItem = doughRecipe.DoughItem;
                    duration = doughRecipe.KneadDuration;
                }
                break;
            case Resolvor.Baking:
                failItem = resolver.FailBakedBreadRecipe.BreadItem;
                duration = resolver.FailBakedBreadRecipe.MinigameBarDuration;
                var bakingRecipe = resolver.ResolveBakedBread(bucketItems[0]);

                if (bakingRecipe)
                {
                    resultItem = bakingRecipe.BreadItem;
                    duration = bakingRecipe.MinigameBarDuration;
                }
                break;
            case Resolvor.Additive:
                failItem = resolver.FailResultBreadRecipe.ResultItem;
                duration = resolver.FailResultBreadRecipe.CompletionDuration;
                var additiveRecipe = resolver.ResolveAdditive(bucketItems[0], bucketItems.GetRange(1, bucketItems.Count - 1));

                if (additiveRecipe)
                {
                    resultItem = additiveRecipe.ResultItem;
                    duration = additiveRecipe.CompletionDuration;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return (failItem, resultItem, duration);
    }

    private IEnumerator CoUpdate(PlayerController pc)
    {
        float t = 0f;
        InputAction keyAction = InputManager.Map.Minigame.BakeryKeyPressed;
        
        _panel.SetActive(true);
        _fillImage.fillAmount = 0f;

        (ItemData failItem, ItemData resultItem, float duration) tuple = GetResolvedItem();

        pc.MoveStrategy.IsStopped = true;
        pc.MoveStrategy.ResetVelocity();

        pc.transform.position = (Vector2)_playPoint.position;
        pc.VisualStrategy.ChangeClip(AnimationActorKey.GetAniHash(AnimationActorKey.Action.Bakery_Knead));

        bool success = false;
        while (true)
        {
            if (keyAction.IsPressed() is false)
            {
                GameReset(tuple, pc);
                
                pc.MoveStrategy.IsStopped = false;
                pc.MoveStrategy.ResetVelocity();
                pc.VisualStrategy.ChangeClip(AnimationActorKey.GetAniHash(AnimationActorKey.Movement.Idle, AnimationActorKey.Direction.Down), true);
                yield break;
            }

            _fillImage.fillAmount = t / tuple.duration;
            t += Time.deltaTime;

            if (t > tuple.duration)
            {
                success = true;
                break;
            }

            yield return null;
        }

        yield return null;
        
        pc.MoveStrategy.IsStopped = false;
        pc.MoveStrategy.ResetVelocity();
        pc.VisualStrategy.ChangeClip(AnimationActorKey.GetAniHash(AnimationActorKey.Movement.Idle, AnimationActorKey.Direction.Down), true);
        
        
        if (success)
        {
            GameSuccess(tuple, pc);
        }
        else
        {
            GameFail(tuple, pc);
        }
        
        GameReset(tuple, pc);
    }

    private void GameReset((ItemData failItem, ItemData resultItem, float duration) tuple, PlayerController pc)
    {
        StopAllCoroutines();
        _panel.SetActive(false);
    }

    private void GameSuccess((ItemData failItem, ItemData resultItem, float duration) tuple, PlayerController pc)
    {
        if (tuple.resultItem == false) return;
        
        bool success = pc.Inventory.Model.PushItem(tuple.resultItem, 1);
        if (success is false) return;
        
        ClearBucket();
    }
    private void GameFail((ItemData failItem, ItemData resultItem, float duration) tuple, PlayerController pc)
    {
        if (tuple.failItem == false) return;
        
        bool success = pc.Inventory.Model.PushItem(tuple.failItem, 1);
        if (success is false) return;
        
        ClearBucket();
    }

    protected override bool CanStore(int index, ItemData itemData)
    {
        var resolver = BakeryRecipeResolver.Instance;
        
        switch (_resolvor)
        {
            case Resolvor.Dough:
                return resolver.CanListOnDough(itemData);
            case Resolvor.Baking:
                return resolver.CanListOnBakedBread(itemData);
            case Resolvor.Additive:
                if (index == 1)
                {
                    return resolver.CanListOnAdditive(itemData);
                }

                return resolver.CanListOnBakedBread(itemData);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}