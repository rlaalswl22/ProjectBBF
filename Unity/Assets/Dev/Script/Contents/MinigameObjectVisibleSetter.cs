using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameObjectVisibleSetter : MonoBehaviour
{
    [Serializable]
    private enum EventType
    {
        None,
        Awake,
        Start,
        Init,
        PreEnd,
        End,
    }
    
    [Serializable]
    private struct EventSet
    {
        public GameObject GameObject;
        public EventType ActiveEvent;
        public EventType InActiveEvent;
    }
    
    [SerializeField] private GameObject _minigameObject;

    [SerializeField] private EventSet[] _sets;

    private IMinigameEventSignal _signal;
    
    private void Awake()
    {
        _signal = _minigameObject.GetComponent<IMinigameEventSignal>();
        if (_signal is null) return;

        _signal.OnGameInitEvent += OnGameInit;
        _signal.OnPreGameEndEvent += OnGamePreEnd;
        _signal.OnGameEndEvent += OnGameEnd;
        
        SetVisibleAll(EventType.Awake);
    }

    private void Start()
    {
        SetVisibleAll(EventType.Start);
    }

    private void SetVisible(EventType currentEvent, EventSet set)
    {
        if (currentEvent == set.ActiveEvent)
        {
            set.GameObject.SetActive(true);
            return;
        }
        if (currentEvent == set.InActiveEvent)
        {
            set.GameObject.SetActive(false);
            return;
        }
    }

    private void SetVisibleAll(EventType currentEvent)
    {
        foreach (var set in _sets)
        {
            if (set.GameObject == false) continue;

            SetVisible(currentEvent, set);
        }
    }

    private void OnGameInit()
    {
        SetVisibleAll(EventType.Init);
    }

    private void OnGameEnd()
    {
        SetVisibleAll(EventType.End);
    }
    private void OnGamePreEnd()
    {
        SetVisibleAll(EventType.PreEnd);
    }
}
