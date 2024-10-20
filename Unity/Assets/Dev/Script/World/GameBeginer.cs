using System;
using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

public class GameBeginer : MonoBehaviour
{
    public enum Type
    {
        Begin,
        Pause,
        Resume,
        End,
    }
    [SerializeField] private Type _type;
    [SerializeField] private bool _useInitTime;
    [SerializeField] private GameTime _initTime;
    
    private void Awake()
    {
        if (_type == Type.Begin)
        {
            TimeManager.Instance.Begin();
        }
        else if (_type == Type.End)
        {
            TimeManager.Instance.End();
        }
        else if (_type == Type.Pause)
        {
            TimeManager.Instance.Pause();
        }
        else if (_type == Type.Resume)
        {
            TimeManager.Instance.Resume();
        }

        if (_useInitTime)
        {
            TimeManager.Instance.SetTime(_initTime);
        }
    }
}
