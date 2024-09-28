using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.Events;

public class MoleGameObject : MonoBehaviour, IBACollectTool
{
    [SerializeField] private CollisionInteraction _interaction;
    [SerializeField] private float _appearAnimationDuration;
    [SerializeField] private float _disapearAnimationDuration;
    public CollisionInteraction Interaction => _interaction;

    private MoleMinigameData _data;
    private MoleMinigameData.Mole _moleData;

    public MoleMinigameData.Mole MoleData => _moleData;

    public UniTaskCompletionSource<MoleMinigameData.Mole> OnHit { get; private set; } = new();

    private CancellationTokenSource _cts;
    public bool IsHit { get; private set; }

    private void Awake()
    {
        var info = ActorContractInfo.Create(()=> gameObject);
        Interaction.SetContractInfo(info, this);

        info.AddBehaivour<IBACollectTool>(this);
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
    }
    
    public async UniTask WaitAppearAsync(CancellationToken token = default)
    {
        if (this == false) return;
        token = CancellationTokenSource.CreateLinkedTokenSource(token, this.GetCancellationTokenOnDestroy(), _cts?.Token ?? default).Token;
        
        
        gameObject.SetActive(true);
        // TODO: Run animation
        var result = await UniTask.Delay(TimeSpan.FromSeconds(_appearAnimationDuration), cancellationToken: token).SuppressCancellationThrow();
        if (result) return;
        
        result = await UniTask.Delay(TimeSpan.FromSeconds(_moleData.WaitDuration), cancellationToken: token).SuppressCancellationThrow();
        if (result) return;
    }

    public async UniTask WaitDisappearAsync(CancellationToken token = default)
    {
        if (this == false) return;
        
        token = CancellationTokenSource.CreateLinkedTokenSource(token, this.GetCancellationTokenOnDestroy(), _cts?.Token ?? default).Token;
        
        // TODO: Run animation
        var result = await UniTask.Delay(TimeSpan.FromSeconds(_disapearAnimationDuration), cancellationToken: token).SuppressCancellationThrow();
        if (result) return;
    }

    public MoleGameObject Clone()
    {
        var obj = Instantiate(this);
        return obj;
    }

    public bool CanCollect(ToolRequireSet toolSet)
    {
        return ToolTypeUtil.Contains(toolSet, _data.RequireTools);
    }

    public List<ItemData> Collect()
    {
        IsHit = true;
        _cts?.Cancel();
        _cts = null;
        return new List<ItemData>(0);
    }
}