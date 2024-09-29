using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField] private PlayerMoneyUI _moneyUI;
    [SerializeField] private TimeHudUI _timeUI;
    [SerializeField] private PlayerHealthUI _healthUI;

    public PlayerMoneyUI MoneyUI => _moneyUI;
    public TimeHudUI TimeUI => _timeUI;
    public PlayerHealthUI HealthUI => _healthUI;

    public bool Visible
    {
        get => gameObject.activeSelf;
        set=> gameObject.SetActive(value);
    }

    public void SetAllHudVisible(bool value)
    {
        MoneyUI.Visible = value;
        TimeUI.Visible = value;
        HealthUI.Visible = value;
    }
}
