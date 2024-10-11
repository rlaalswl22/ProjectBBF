using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;

public interface IBAInteractionTrigger: IActorBehaviour
{
    public bool Interact(CollisionInteractionMono caller);
}

public class MapInteractionTrigger : MapTriggerBase, IBAInteractionTrigger
{
    public bool Interact(CollisionInteractionMono caller)
    {
        Trigger(caller);

        return true;
    }

    protected override void Awake()
    {
        base.Awake();
        ContractInfo.AddBehaivour<IBAInteractionTrigger>(this);
    }
}
