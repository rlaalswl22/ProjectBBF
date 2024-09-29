using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Exception = System.Exception;
using Random = UnityEngine.Random;


public class FishingContext
{
    private SpriteRenderer _renderer;
    private FishingMinigameData _data;

    public event Action<FishingContext> OnBeginBite;
    public event Action<FishingContext> OnEndBite;
    
    public FishingContext(SpriteRenderer renderer, FishingMinigameData data)
    {
        _renderer = renderer;
        _renderer.enabled = false;
        _data = data;
    }

    

    public bool CanFishingGround(Vector2 position)
    {
        Collider2D collider = Physics2D.OverlapBox(position, Vector2.one, 0f, LayerMask.GetMask("FishingField"));
        return collider;
    }
    
    public bool IsBite { get; private set; }

    public ItemData Reward { get; private set; }

    public bool FishVisible
    {
        get => _renderer.enabled;
        set=> _renderer.enabled = value;
    }
    public Transform FishTransform { get; private set; }
    public bool IsTiming => Reward is not null && FishTransform is not null;
    
    private CancellationTokenSource _cts;
    private bool _isStopped;

    public CancellationToken CancellationToken
    {
        get
        {
            if (_cts is null)
            {
                Debug.LogError("FishingContext is not initialized");
                return default;
            }
            
            return _cts.Token;
        }
    }

    public void Pause()
    {
        _isStopped = true;
    }

    public void Resume()
    {
        _isStopped = false;
    }
    
    public async UniTask Begin(CancellationToken token = default)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(
            token, GlobalCancelation.PlayMode
        );

        token = _cts.Token;
        
        try
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_data.BiteRepeatInterval), false, PlayerLoopTiming.Update, token);
                await UniTask.WaitWhile(() => _isStopped, cancellationToken: token);
                
                OnBeginBite?.Invoke(this);

                ItemData reward = GetReward();

                FishTransform = _renderer.transform;
                Reward = reward;
            
                await UniTask.Delay(TimeSpan.FromSeconds(_data.BiteCanBiteDuration), false, PlayerLoopTiming.Update, token);
                await UniTask.WaitWhile(() => _isStopped, cancellationToken: token);
                
                FishTransform = null;
                Reward = null;
                OnEndBite?.Invoke(this);
            }
        }
        catch(Exception e) when (e is not OperationCanceledException)
        {
            Debug.LogException(e);
        }
    }

    public void Release()
    {
        _renderer.enabled = false;
        _cts?.Cancel();
        
        IsBite = false;
        FishTransform = null;
        _renderer = null;
        Reward = null;
        OnBeginBite = null;
        
        OnEndBite?.Invoke(this);
        OnEndBite = null;
    }

    private ItemData GetReward()
    {
        var v = Random.value;

        float sum = 0f;
        foreach (var reward in _data.Rewards)
        {
            sum += reward.Percentage;

            if (sum >= v)
            {
                return reward.Item;
            }
        }
        
        if (_data.Rewards.Count == 0)
        {
            throw new Exception(); 
        }        
        
        return _data.Rewards[0].Item;
    }
}