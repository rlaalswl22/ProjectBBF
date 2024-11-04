using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapInteractionTrigger : MapTriggerBase, IBOInteractive
{
    [SerializeField] private InputAction _triggerInput;

    protected override void Awake()
    {
        base.Awake();

        ContractInfo.AddBehaivour<IBOInteractive>(this);
    }

    public void UpdateInteract(CollisionInteractionMono caller)
    {
        if (_triggerInput is null) return;

        if (_triggerInput.triggered)
        {
            Trigger(caller);
        }
    }
}
