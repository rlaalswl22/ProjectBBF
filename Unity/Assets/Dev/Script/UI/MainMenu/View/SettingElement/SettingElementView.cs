using UnityEngine;
using System;
using UnityEngine.Events;


public abstract class BaseSettingElementView : MonoBehaviour
{
    public object Data { get; set; }
    public abstract string GroupKey { get; protected set; }
    public abstract string Key { get; protected set; }

    public abstract void Init();
    public abstract void Release();
    public abstract void ClearEvent();
}

public abstract class SettingElementView<TView, TValue> : BaseSettingElementView
    where TView : SettingElementView<TView, TValue>
    where TValue : struct
{
    [SerializeField] private UnityEvent<TView, TValue> _onChangedValue;
    
    public event UnityAction<TView, TValue> OnChangedValue
    {
        add => _onChangedValue.AddListener(value);
        remove => _onChangedValue.RemoveListener(value);
    }

    protected void Raise(TValue value)
    {
        _onChangedValue?.Invoke(this as TView, value);
    }

    public override void ClearEvent()
    {
        _onChangedValue.RemoveAllListeners();
    }

    public abstract void SetValue(TValue value, bool withoutNotify = false);
    public abstract TValue GetValue();
}