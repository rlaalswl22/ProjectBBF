using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    // sound
    [SerializeField] private List<BaseSettingElementView> _elementViews;

    public event Action<(string settingGroupKey, string groupKey, object value)> OnChangedElementValue;

    public event Action<bool> OnChangedVisible;
    

    public bool Visible
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    private void OnEnable()
    {
        OnChangedVisible?.Invoke(true);
    }
    private void OnDisable()
    {
        OnChangedVisible?.Invoke(false);
    }

    public void Init()
    {
        _elementViews.ForEach(x =>
        {
            x.Init();
            
            if (x is SliderSettingElementView sliderView)
            {
                sliderView.OnChangedValue += Raise;
            }
            else if (x is DropdownSettingElementView dropdownView)
            {
                dropdownView.OnChangedValue += Raise;
            }
        });
    }

    public void Release()
    {
        OnChangedElementValue = null;
        
        _elementViews.ForEach(x =>
        {
            x.Init();
            x.ClearEvent();
        });
    }
    
    private void Raise(SliderSettingElementView view, float value)
    {
        OnChangedElementValue?.Invoke((view.GroupKey, view.Key, value));
    }
    private void Raise(DropdownSettingElementView view, int value)
    {
        OnChangedElementValue?.Invoke((view.GroupKey, view.Key, value));
    }

    public bool TryElementView<T>(string groupKey, string key, out T elementView)
        where T : BaseSettingElementView
    {
        BaseSettingElementView baseView = _elementViews.FirstOrDefault(x => x.GroupKey == groupKey && x.Key == key);
        elementView = baseView as T;

        return elementView;
    }

    public IReadOnlyList<T> GetElementViewAll<T>(string groupKey)
        where T : BaseSettingElementView
    {
        return _elementViews
            .Where(x => x is T && x.GroupKey == groupKey)
            .Cast<T>()
            .ToList();
    }
}