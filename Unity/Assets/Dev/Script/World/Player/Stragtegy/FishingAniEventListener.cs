using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishingAniEventListener : MonoBehaviour
{
    [SerializeField] private UnityEvent _onStartFishingLine; 
    
    public void Begin()
    {
        _onStartFishingLine?.Invoke();
    }
}
