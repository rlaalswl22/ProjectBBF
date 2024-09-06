using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;

public class ActorFavorablity: MonoBehaviour, IActorStrategy, IBAFavorablity
{
    private Actor _actor;
    
    public CollisionInteraction Interaction => _actor.Interaction;
    public FavorablityContainer FavorablityContainer { get; private set; }
    
    public void Init(Actor actor)
    {
        _actor = actor;
        
        if (ActorDataManager.Instance.CachedDict.TryGetValue(actor.ActorKey, out var data))
        {
            FavorablityContainer = new FavorablityContainer(data.FavorabilityEvent, 0, null);
        }
        else
        {
            Debug.LogError($"actor key({actor.ActorKey})를 찾을 수 없음");
        }
    }
}