using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public bool Visible
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    private int _score;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            
            if (_text)
            {
                _text.text = _score.ToString();
            } 
        }
    }

    private void Start()
    {
        //Visible = false;
    }
}
