using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class MapCollisionTrigger : MapTriggerBase
{
    private void OnEnter(ActorContractInfo info)
    {
        if (info.Interaction.Owner is PlayerController pc)
        {
            Trigger(pc.Interaction);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Interaction.OnContractActor += OnEnter;
    }

    private void OnDestroy()
    {
        Interaction.OnContractActor -= OnEnter;
    }
}
