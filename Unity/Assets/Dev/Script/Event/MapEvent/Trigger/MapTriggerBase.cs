using System;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;

[RequireComponent(typeof(CollisionInteraction))]
public class MapTriggerBase : MonoBehaviour
{
    [field: SerializeField, AutoProperty] 
    private CollisionInteraction _interaction;

    public CollisionInteraction Interaction => _interaction;
    public ActorContractInfo ContractInfo => Interaction.ContractInfo as ActorContractInfo;
    
    public event Action<CollisionInteractionMono> OnTrigger;

    protected void Trigger(CollisionInteractionMono caller)
    {
        OnTrigger?.Invoke(caller);
    }

    protected virtual void Awake()
    {
        var info = ActorContractInfo.Create(() => this);
        Interaction.SetContractInfo(info, this);
    }
}