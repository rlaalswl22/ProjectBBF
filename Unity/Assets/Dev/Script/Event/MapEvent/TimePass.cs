using System;
using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using ProjectBBF.Persistence;
using UnityEngine;


public class TimePassPersistenceObject
{
    [NonSerialized] public int CurrentTime;
}

public class TimePass : MonoBehaviour
{
    [SerializeField] private MoveToWorld _moveToWorld;
    [SerializeField] private int _goalTime;

    private  TimePassPersistenceObject _persistenceObject;
    private PlayerController _player;

    private void Awake()
    {
        _persistenceObject = PersistenceManager.Instance.LoadOrCreate<TimePassPersistenceObject>("TimePass");

        if (_persistenceObject.CurrentTime <= 0)
        {
            _persistenceObject.CurrentTime = _goalTime;
        }
        StartCoroutine(CoInitPlayer());
    }

    private IEnumerator CoInitPlayer()
    {
        while (true)
        {
            if (GameObjectStorage.Instance.TryGetPlayerController(out _player))
            {
                _player.HudController.TimeUI.OverrideTimeText = true;
                _player.HudController.TimeUI.TimeText = $"{_persistenceObject.CurrentTime} 분";
                break;
            }

            yield return null;
        }
    }
    
    public void Pass(IntEvent evt)
    {
        if (_player == false) return;

        _persistenceObject.CurrentTime -= evt.Value;
        
        _persistenceObject.CurrentTime = Mathf.Max(0, _persistenceObject.CurrentTime);
        
        _player.HudController.TimeUI.OverrideTimeText = true;
        _player.HudController.TimeUI.TimeText = $"{_persistenceObject.CurrentTime} 분";
        
        if (_persistenceObject.CurrentTime <= 0 && _moveToWorld)
        {
            _moveToWorld.MoveWorld();
        }
    }

    public void Pass(ESOTimeIntVoid eso)
    {
        Pass(new IntEvent()
        {
            Value = eso.Value
            
        });
    }
}
