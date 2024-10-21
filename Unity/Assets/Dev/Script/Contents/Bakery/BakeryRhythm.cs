using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class BakeryRhythm : BakeryFlowBehaviourBucket, IObjectBehaviour
{
    [SerializeField] private GameObject _activationUI;
    [SerializeField] private AudioSource _source;
    [SerializeField] private ESOVoid _esoSuccess;
    
    [SerializeField] private float _roundTripInterval;
    [SerializeField] private Transform _playPoint;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _tipImage;
    [SerializeField] private Image _boundBoxImage;
    [SerializeField] private Vector2 _tipStartPos;
    [SerializeField] private Vector2 _tipEndPos;

    [SerializeField] private Transform _fireContent;
    [SerializeField] private float _fireLargeScale;
    [SerializeField] private float _fireFadeinDuration;
    [SerializeField] private float _fireAnimationKeepDuration;
    [SerializeField] private float _fireAnimationFadeoutDuration;
    [SerializeField] private Ease _fadeinEase;
    [SerializeField] private Ease _fadeoutEase;

    [SerializeField] private float _keyPressAndShakeAngle = 25f;
    [SerializeField] private float _keyPressAndShakeDuration = 0.5f;
    
    [SerializeField] private float _keyPressAndScaleValue = 1.5f;
    [SerializeField] private float _keyPressAndScaleDuration = 0.5f;
    
    

    private int FAIL_GOAL_COUNT = 3;
    private int SUCCESS_GOAL_COUNT = 3;

    private bool _isPlaying;

    private void Start()
    {
        _activationUI.SetActive(false);
        _panel.SetActive(false);
        SetVisibleFire(false);
    }

    private void SetVisibleFire(bool value)
    {
        for (int i = 0; i < _fireContent.childCount; i++)
        {
            _fireContent.GetChild(i).gameObject.SetActive(value);
        }
    }

    private void DoAnimateFire(bool makingLarge, bool immediate = false)
    {
        DOTween.Kill(this);
        
        Vector3 scale = makingLarge ? (Vector3.one * _fireLargeScale) : Vector3.one;
        
        for (int i = 0; i < _fireContent.childCount; i++)
        {
            var t = _fireContent.GetChild(i);
            if (immediate is false && makingLarge)
            {
                Sequence s = DOTween.Sequence();

                s.Append(t.DOScale(_fireLargeScale, _fireFadeinDuration).SetEase(_fadeinEase));
                s.AppendInterval(_fireAnimationKeepDuration);
                s.Append(t.DOScale(Vector3.one, _fireAnimationFadeoutDuration).SetEase(_fadeoutEase));
                s.SetId(this);
                s.Play();
            }
            else
            {
                t.localScale = scale;
            }
        }
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
        if (IsFullBucket)
        {
            _activationUI.SetActive(true);
        }
    }

    protected override void OnExit(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
        _activationUI.SetActive(false);
        GameReset();
        StopAllCoroutines();
    }

    protected override void OnChangedBuket(int index, ItemData itemData)
    {
        base.OnChangedBuket(index, itemData);

        for (int i = 0; i < BucketLength; i++)
        {
            if (GetBucket(i) == false)
            {
                _activationUI.SetActive(false);
                return;
            }
        }
        
        _activationUI.SetActive(true);
    }

    private IEnumerator CoUpdate(PlayerController pc)
    {
        float t = 0f;
        float dir = 1f;

        int successCount = 0;
        int failCount = 0;

        var inputAction = InputManager.Map.Minigame.BakeryOvenHit;
        (ItemData failItem, ItemData resultItem, float duration, BakeryRecipeData recipe) tuple = GetResolvedItem();
        

        _activationUI.SetActive(false);
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
                
                _source.Stop();
                _source.Play("SFX", "SFX_Bakery_OvenFire");

                if (tip.Intersects(bound))
                {
                    successCount++;
                    DoAnimateFire(true);
                    yield return _panel
                        .transform
                        .DOScaleY(_keyPressAndScaleValue, _keyPressAndScaleDuration).SetId(this)
                        .SetLoops(2, LoopType.Yoyo)
                        .OnComplete(()=>_panel.transform.localScale = Vector3.one)
                        .WaitForCompletion();
                }
                else
                {
                    failCount++;
                    yield return _panel
                        .transform
                        .DOShakeRotation(_keyPressAndShakeDuration, new Vector3(0f, 0f, _keyPressAndShakeAngle))
                        .SetId(this).OnComplete(()=>_panel.transform.rotation = Quaternion.identity)
                        .WaitForCompletion();
                }

                if (tuple.failItem && failCount >= FAIL_GOAL_COUNT)
                {
                    GameFail(tuple, pc);
                    break;
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

            if (t >= 1f)
            {
                t = 1f;
                dir = -1f;
            }
            else if (t <= 0f)
            {
                t = 0f;
                dir = 1f;
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
        SetVisibleFire(true);
        _source.Stop();
    }
    private void GameReset()
    {
        _isPlaying = false;
        _panel.SetActive(false);
        SetVisibleFire(false);
        _source.Stop();
    }


    private void GameSuccess((ItemData failItem, ItemData resultItem, float duration, BakeryRecipeData recipe) tuple, PlayerController pc)
    {
        if (tuple.resultItem == false) return;
        
        bool success = pc.Inventory.Model.PushItem(tuple.resultItem, 1);
        ClearBucket();

        if (success && tuple.recipe)
        {
            pc.RecipeBookPresenter.Model.Add(tuple.recipe.Key);
        }

        if (_esoSuccess)
        {
            _esoSuccess.Raise();
        }
    }
    private void GameFail((ItemData failItem, ItemData resultItem, float duration, BakeryRecipeData recipe)tuple, PlayerController pc)
    {
        if (tuple.failItem == false) return;
        
        bool success = pc.Inventory.Model.PushItem(tuple.failItem, 1);
        if (success is false) return;
        
        ClearBucket();
    }


}