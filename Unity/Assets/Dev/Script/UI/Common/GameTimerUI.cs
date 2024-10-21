using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimerUI : MonoBehaviour
{
    [SerializeField] private bool _startAndInvisible;
    [SerializeField] private TMP_Text _text;

    public bool Visible
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    private GameTime _time;

    public GameTime Time
    {
        get => _time;
        set
        {
            _time = value;
            
            if (_text)
            {
                _text.text = _time.ToStringFormat("{0:D2} : {1:D2}");
            } 
        }
    }

    private void Start()
    {
        if (_startAndInvisible)
        {
            Visible = false;
        }
    }
}
