using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;

public class MapEventListenerTrigger : MapTriggerBase
{
    [SerializeField] private ESOVoid _esoVoid;

    protected override void Awake()
    {
        base.Awake();

        _esoVoid.OnEventRaised += OnTrigger;
    }

    private void OnDestroy()
    {
        _esoVoid.OnEventRaised -= OnTrigger;
    }

    private void OnTrigger()
    {
        var player = GameObjectStorage.Instance.StoredObjects.FirstOrDefault(x => x.CompareTag("Player"));

        if (player && player.TryGetComponent(out PlayerController pc))
        {
            Trigger(pc.Interaction);
        }
    }
}
