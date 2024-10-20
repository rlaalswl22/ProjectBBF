using System;
using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

public class TimePass : MonoBehaviour
{
    [SerializeField] private MoveToWorld _moveToWorld;
    [SerializeField] private int _goalTime;

    private int _currentTime;    
    private PlayerController _player;

    private void Awake()
    {
        _currentTime = _goalTime;
        StartCoroutine(CoInitPlayer());
    }

    private IEnumerator CoInitPlayer()
    {
        while (true)
        {
            if (GameObjectStorage.Instance.TryGetPlayerController(out _player))
            {
                _player.HudController.TimeUI.OverrideTimeText = true;
                _player.HudController.TimeUI.TimeText = $"{_currentTime} 분";
                break;
            }

            yield return null;
        }
    }
    
    public void Pass(IntEvent evt)
    {
        if (_player == false) return;

        _currentTime -= evt.Value;
        
        _currentTime = Mathf.Max(0, _currentTime);
        
        _player.HudController.TimeUI.OverrideTimeText = true;
        _player.HudController.TimeUI.TimeText = $"{_currentTime} 분";
        
        if (_currentTime <= 0 && _moveToWorld)
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
