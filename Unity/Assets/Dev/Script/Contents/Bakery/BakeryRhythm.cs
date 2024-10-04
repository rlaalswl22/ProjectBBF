using System;
using System.Collections;
using System.Linq;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class BakeryRhythm : BakeryFlowBehaviourBucket, IObjectBehaviour
{
    [SerializeField] private float _roundTripInterval;
    [SerializeField] private Transform _playPoint;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _tipImage;
    [SerializeField] private Image _boundBoxImage;
    [SerializeField] private Vector2 _tipStartPos;
    [SerializeField] private Vector2 _tipEndPos;

    private int FAIL_GOAL_COUNT = 3;
    private int SUCCESS_GOAL_COUNT = 3;

    private bool _isPlaying;

    private void Start()
    {
        _panel.SetActive(false);
    }

    protected override void OnActivate(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
        if (activator.Owner is not PlayerController pc) return;
        if (IsFullBucket is false) return;

        if (_isPlaying) return;

        GameSetup();
        StartCoroutine(CoUpdate(pc));
    }

    protected override void OnEnter(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
    }

    protected override void OnExit(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
        GameReset();
        StopAllCoroutines();
    }

    private IEnumerator CoUpdate(PlayerController pc)
    {
        float t = 0f;
        float dir = 1f;

        int successCount = 0;
        int failCount = 0;

        var inputAction = InputManager.Map.Minigame.BakeryOvenHit;
        (ItemData failItem, ItemData resultItem, float duration, BakeryRecipeData recipe) tuple = GetResolvedItem();
        

        pc.Blackboard.IsMoveStopped = true;
        pc.Blackboard.IsInteractionStopped = true;
        pc.transform.position = (Vector2)_playPoint.position;
        pc.MoveStrategy.ResetVelocity();
        pc.VisualStrategy.ChangeClip(AnimationActorKey.GetAniHash(AnimationActorKey.Action.Bakery_Additive, AnimationActorKey.Direction.Down));
        
        while (true)
        {
            if (inputAction.triggered)
            {
                var tipTransform = _tipImage.transform;
                var boundTransform = _boundBoxImage.transform;
                
                Bounds tip = new Bounds(
                    (Vector2)tipTransform.position,
                    tipTransform.lossyScale * ((RectTransform)tipTransform).rect.width
                );
                Bounds bound = new Bounds(
                    (Vector2)boundTransform.position,
                    boundTransform.lossyScale * ((RectTransform)boundTransform).rect.width
                );

                if (tip.Intersects(bound))
                {
                    successCount++;
                }
                else
                {
                    failCount++;
                }

                if (successCount >= SUCCESS_GOAL_COUNT)
                {
                    pc.VisualStrategy.ChangeClip(AnimationActorKey.GetAniHash(AnimationActorKey.Action.Bakery_Additive_Complete, AnimationActorKey.Direction.Down));
                    GameSuccess(tuple, pc);
                    break;
                }

                if (failCount >= FAIL_GOAL_COUNT)
                {
                    GameFail(tuple, pc);
                    break;
                }
            }
            
            _tipImage.transform.localPosition = Vector2.Lerp(_tipStartPos, _tipEndPos, t);

            t += Time.deltaTime / (_roundTripInterval * 0.5f * dir);

            if (t >= 1f || t <= 0f)
            {
                dir *= -1f;
            }

            yield return null;
        }
        
        
        pc.Blackboard.IsMoveStopped = false;
        pc.Blackboard.IsInteractionStopped = false;
        pc.MoveStrategy.ResetVelocity();
        pc.VisualStrategy.ChangeClip(AnimationActorKey.GetAniHash(AnimationActorKey.Action.Idle, AnimationActorKey.Direction.Down));
        
        GameReset();
    }

    private void GameSetup()
    {
        _isPlaying = true;
        _panel.SetActive(true);
        _tipImage.transform.position = _tipStartPos;
    }
    private void GameReset()
    {
        _isPlaying = false;
        _panel.SetActive(false);
    }


    private void GameSuccess((ItemData failItem, ItemData resultItem, float duration, BakeryRecipeData recipe) tuple, PlayerController pc)
    {
        if (tuple.resultItem == false) return;
        
        bool success = pc.Inventory.Model.PushItem(tuple.resultItem, 1);
        ClearBucket();
    }
    private void GameFail((ItemData failItem, ItemData resultItem, float duration, BakeryRecipeData recipe)tuple, PlayerController pc)
    {
        if (tuple.failItem == false) return;
        
        bool success = pc.Inventory.Model.PushItem(tuple.failItem, 1);
        if (success is false) return;
        
        ClearBucket();
    }


}