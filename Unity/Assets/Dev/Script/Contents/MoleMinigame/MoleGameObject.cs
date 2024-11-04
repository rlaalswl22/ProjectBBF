using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.Events;

public class MoleGameObject : MonoBehaviour, IBOInteractiveTool
{
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private ParticleSystem _ringEffect;
    [SerializeField] private Animator _ani;
    [SerializeField] private CollisionInteraction _interaction;
    [SerializeField] private float _appearAnimationDuration;
    [SerializeField] private float _disapearAnimationDuration;
    [SerializeField] private float _disapearHitAnimationDuration;
    public CollisionInteraction Interaction => _interaction;

    private MoleMinigameData _data;
    private MoleMinigameData.Mole _moleData;

    public MoleMinigameData.Mole MoleData => _moleData;

    public UniTaskCompletionSource<MoleMinigameData.Mole> OnHit { get; private set; } = new();

    private CancellationTokenSource _cts;
    private static readonly int GetoutAniHash = Animator.StringToHash("Getout");
    private static readonly int IsHitAniHash = Animator.StringToHash("IsHit");
    private static readonly int GetinAniHash = Animator.StringToHash("Getin");
    public bool IsHit { get; private set; }

    private void Awake()
    {
        var info = ObjectContractInfo.Create(()=> gameObject);
        Interaction.SetContractInfo(info, this);

        info.AddBehaivour<IBOInteractiveTool>(this);
    }

    public void Init(MoleMinigameData data, MoleMinigameData.Mole moleData)
    {
        _data = data;
        _moleData = moleData;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void ResetObj()
    {
        _cts = new();
        gameObject.SetActive(false);
        IsHit = false;
        _ani.SetBool(IsHitAniHash, false);
        _hitEffect.Stop();
        _ringEffect.Stop();
    }
    
    public async UniTask WaitAppearAsync(CancellationToken token = default)
    {
        if (this == false) return;
        token = CancellationTokenSource.CreateLinkedTokenSource(token, this.GetCancellationTokenOnDestroy(), _cts?.Token ?? default).Token;
        
        
        gameObject.SetActive(true);
        _ani.SetTrigger(GetoutAniHash);
        AudioManager.Instance.PlayOneShot("Animal", "Animal_Mole_Getout");
        var result = await UniTask.Delay(TimeSpan.FromSeconds(_appearAnimationDuration), cancellationToken: token).SuppressCancellationThrow();
        if (result) return;
        
        result = await UniTask.Delay(TimeSpan.FromSeconds(_moleData.WaitDuration), cancellationToken: token).SuppressCancellationThrow();
        if (result) return;
    }

    public async UniTask WaitDisappearAsync(CancellationToken token = default)
    {
        if (this == false) return;
        
        token = CancellationTokenSource.CreateLinkedTokenSource(token, this.GetCancellationTokenOnDestroy(), _cts?.Token ?? default).Token;
        
        _ani.SetTrigger(GetinAniHash);


        bool result = false;
        if (IsHit is false)
        {
            result = await UniTask.Delay(TimeSpan.FromSeconds(_disapearAnimationDuration), cancellationToken: token).SuppressCancellationThrow();
        }
        else
        {
            result = await UniTask.Delay(TimeSpan.FromSeconds(_disapearHitAnimationDuration), cancellationToken: token).SuppressCancellationThrow();
        }
        
        AudioManager.Instance.PlayOneShot("Animal", "Animal_Mole_Getin");

        if (_hitEffect)
        {
            _hitEffect.Stop();
        }
        if (_ringEffect)
        {
            _ringEffect.Stop();
        }
        if (result) return;
    }

    public MoleGameObject Clone()
    {
        var obj = Instantiate(this);
        return obj;
    }

    public void UpdateInteract(CollisionInteractionMono caller)
    {
        if (caller.Owner is not PlayerController) return;
        
        IsHit = true;
        _cts?.Cancel();
        _cts = null;
        _ani.SetBool(IsHitAniHash, true);
        AudioManager.Instance.PlayOneShot("Animal", "Animal_Mole_Hitted");
        _hitEffect.Play();
        _ringEffect.Play();
    }

    public bool IsVaildTool(ToolRequireSet toolSet)
    {
        return ToolTypeUtil.Contains(toolSet, _data.RequireTools);
    }
}