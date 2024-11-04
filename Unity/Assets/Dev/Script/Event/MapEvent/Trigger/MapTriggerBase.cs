using System;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CollisionInteraction))]
public class MapTriggerBase : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTriggerEvent;
    
    [field: SerializeField, AutoProperty] 
    private CollisionInteraction _interaction;

    public CollisionInteraction Interaction => _interaction;
    public ObjectContractInfo ContractInfo => Interaction.ContractInfo as ObjectContractInfo;
    
    public event Action<CollisionInteractionMono> OnTrigger;

    protected void Trigger(CollisionInteractionMono caller)
    {
        OnTrigger?.Invoke(caller);
        OnTriggerEvent?.Invoke();
    }

    protected virtual void Awake()
    {
        var info = ObjectContractInfo.Create(() => this);
        Interaction.SetContractInfo(info, this);
    }
}