using System;
using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

public class GameBeginer : MonoBehaviour
{
    private void Awake()
    {
        TimeManager.Instance.Begin();
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance == false) return;
        
        TimeManager.Instance.End();
    }
}
