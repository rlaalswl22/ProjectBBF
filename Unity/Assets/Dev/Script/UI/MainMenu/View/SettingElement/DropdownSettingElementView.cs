using UnityEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;


public class DropdownSettingElementView : SettingElementView<DropdownSettingElementView, int>
{
    [SerializeField] private string _groupKey;
    [SerializeField] private string _key;
    [SerializeField] private TMP_Dropdown _dropdown;


    public override string GroupKey
    {
        get => _groupKey;
        protected set => _groupKey = value;
    }
    public override string Key
    {
        get => _key;
        protected set => _key = value;
    }

    
    public override void Init()
    {
        _dropdown.onValueChanged.AddListener(OnSliderChanged);
    }

    public override void Release()
    {
        _dropdown.onValueChanged.RemoveAllListeners();
    }

    private void OnSliderChanged(int value)
    {
        Raise(value);
    }

    public void SetDropdownItem(List<TMP_Dropdown.OptionData> optionData)
    {
        _dropdown.ClearOptions();
        _dropdown.AddOptions(optionData);
    }
    
    public override void SetValue(int value, bool withoutNotify = false)
    {
        _dropdown.SetValueWithoutNotify(value);

        if (withoutNotify is false)
        {
            Raise(value);
        }
    }

    public override int GetValue()
    {
        return _dropdown.value;
    }
}