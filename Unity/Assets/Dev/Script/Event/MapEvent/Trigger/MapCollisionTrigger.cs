using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class MapCollisionTrigger : MapTriggerBase
{
    private void OnEnter(BaseContractInfo info)
    {
        if (info.Interaction.Owner is PlayerController pc)
        {
            Trigger(pc.Interaction);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Interaction.OnContract += OnEnter;
    }

    private void OnDestroy()
    {
        Interaction.OnContract -= OnEnter;
    }
}
