using System;
using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using TMPro;
using UnityEngine;

public class PlayerMoneyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    
    private PlayerBlackboard _blackboard;

    private PlayerBlackboard Blackboard
    {
        get
        {
            if (_blackboard is null)
            {
                _blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");
            }

            return _blackboard;
        }
    }

    public bool Visible
    {
        get=> gameObject.activeSelf;
        set=> gameObject.SetActive(value);
    }

    private void Update()
    {
        _text.text = $"{Blackboard.Money} 원";
    }
}
