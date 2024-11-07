using UnityEngine;
using System;
using System.Globalization;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;


public class SliderSettingElementView : SettingElementView<SliderSettingElementView, float>
{
    [SerializeField] private bool _castInt = true;
    [SerializeField] private string _groupKey;
    [SerializeField] private string _key;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Slider _slider;


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
        _slider.onValueChanged.AddListener(OnSliderChanged);
    }

    public override void Release()
    {
        _slider.onValueChanged.RemoveAllListeners();
    }

    private void OnSliderChanged(float value)
    {
        SetValue(value, true);
        Raise(value);
    }

    public float? MaxValue { get; set; }
    public override void SetValue(float value, bool withoutNotify = false)
    {
        _slider.SetValueWithoutNotify(value);

        if (MaxValue == null)
        {
            MaxValue = 100f;
        }

        if (_castInt)
        {
            _inputField.text = $"{(int)(value * MaxValue.Value)} / {(int)MaxValue.Value}";
        }
        else
        {
            _inputField.text = $"{value * MaxValue.Value:0.00} / {MaxValue.Value}";
        }
        
        if (withoutNotify is false)
        {
            Raise(value);
        }
    }

    public override float GetValue()
    {
        return _slider.value;
    }
}