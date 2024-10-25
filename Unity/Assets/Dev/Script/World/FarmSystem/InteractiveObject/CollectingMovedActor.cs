using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Annotations;
using ProjectBBF.Event;
using MyBox;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CollectingMovedActor : ActorComponent
{
    
    [field: SerializeField, InitializationField]
    private CollectingObjectData _collectingData;

    [SerializeField] private List<ESOVoid> _collectAndRaisingEvents;
    [SerializeField] private UnityEvent<CollectState> _onChangedCollectingState;

    [SerializeField] private string _collectedPlayingAudioGroupKey;
    [SerializeField] private string _collectedPlayingAudioKey;
    
    [field: SerializeField, InitializationField, MustBeAssigned]
    private List<ESOGameTimeEvent> _refillEvents;
    
    //[SerializeField] private GameObject  

    private int _collectCount;
    private PatrolPointPath _currentPath;
    private Actor _masterActor;
    public bool CanCollect => _collectCount < _collectingData.MaxCollectCount;

    public CollectingObjectData CollectingData => _collectingData;
    public CollisionInteraction Interaction => _masterActor.Interaction;

    public UnityEvent<CollectState> OnChangedCollectingState => _onChangedCollectingState;

    private class ToolBehaviour : IBACollectTool
    {
        public CollectingMovedActor _actorCom;
        public CollisionInteraction Interaction { get; set; }
        public bool CanCollect(ToolRequireSet toolSet)
        {
            return ToolTypeUtil.Contains(_actorCom._collectingData.RequireSet, toolSet);
        }

        public List<ItemData> Collect() => _actorCom.Collect();
    }
    private class CollectBehaviour : IBACollect
    {
        public CollectingMovedActor _actorCom;
        public CollisionInteraction Interaction { get; set; }


        public List<ItemData> Collect() => _actorCom.Collect();
    }

    public IActorBehaviour CreateCollectProxyOrNull()
    {
        if (CollectingData)
        {
            if (CollectingData.OnlyTool)
            {
                return new ToolBehaviour()
                {
                    Interaction = _masterActor.Interaction,
                    _actorCom = this
                };
            }
            else
            {
                return new CollectBehaviour()
                {
                    Interaction = _masterActor.Interaction,
                    _actorCom = this
                };
            }
        }

        return null;
    }
    
    public void Init(Actor actor)
    {
        _masterActor = actor;
        foreach (var refillEvent in _refillEvents)
        {
            if (refillEvent == false) continue;
            refillEvent.OnSignal += Refill;
        }
    }
    private void OnDestroy()
    {
        foreach (var refillEvent in _refillEvents)
        {
            if (refillEvent == false) continue;
            refillEvent.OnSignal -= Refill;
        }
    }

    private void Refill(GameTime obj = default)
    {
        _onChangedCollectingState.Invoke(CollectState.Normal);
        _collectCount = 0;
    }
    
    public List<ItemData> Collect()
    {
        if (_collectingData == false) return null;
        if (_collectCount >= CollectingData.MaxCollectCount) return null;
            
        var list = new List<ItemData>();
        if (CanCollect is false) return null;

        _collectCount++;

        if (string.IsNullOrEmpty(_collectedPlayingAudioGroupKey) is false && 
            string.IsNullOrEmpty(_collectedPlayingAudioKey) is false)
        {
            string groupKey = _collectedPlayingAudioGroupKey.Trim();
            string audiokey = _collectedPlayingAudioKey.Trim();
            AudioManager.Instance.PlayOneShot(groupKey, audiokey);
        }

        
        foreach (var item in _collectingData.DropItems)
        {
            for (int i = 0; i < item.Count; i++)
            {
                list.Add(item.Data);
            }
        }
        

        foreach (var eso in _collectAndRaisingEvents)
        {
            if (eso)
            {
                eso.Raise();
            }
        }

        if (_collectCount >= CollectingData.MaxCollectCount)
        {
            _onChangedCollectingState.Invoke(CollectState.Collected);
        }

        return list;
    }
}