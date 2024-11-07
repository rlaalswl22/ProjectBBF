using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPresenter : MonoBehaviour
{
    [SerializeField] private SettingView _view;

    private SettingAudioModel _audioModel;
    private SettingGraphicModel _graphicModel;
    
    private void Awake()
    {
        _audioModel = new();
        _audioModel.OnChangedVolume += OnChangedModelAudio;
        
        _view.Init();
        _view.OnChangedVisible += OnChangedViewVisible;
        _view.OnChangedElementValue += OnChangedViewAudio;
        _view.OnChangedElementValue += OnChangedViewGraphic;

        foreach (SliderSettingElementView elementView in _view.GetElementViewAll<SliderSettingElementView>("Audio"))
        {
            float volume = _audioModel.GetVolume(elementView.Key);
            elementView.SetValue(volume, true);
        }


        _graphicModel = new();
        _graphicModel.OnChangedGraphic += OnChangedModelGraphic;
        
        foreach (DropdownSettingElementView elementView in _view.GetElementViewAll<DropdownSettingElementView>("Graphic"))
        {
            int indexCount = _graphicModel.GetIndexCount(elementView.Key);
            List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>(indexCount);
            for (int i = 0; i < indexCount; i++)
            {
                string text = _graphicModel.GetItemName(elementView.Key, i);
                list.Add(new TMP_Dropdown.OptionData(text));
            }

            int currentIndex = _graphicModel.GetIndex(elementView.Key);
            elementView.SetDropdownItem(list);
            elementView.SetValue(currentIndex);
        }

        foreach (SliderSettingElementView elementView in _view.GetElementViewAll<SliderSettingElementView>("Graphic"))
        {
            (float normalizedValue, float? originMaxValue) tuple = _graphicModel.GetSliderValue(elementView.Key);
            elementView.MaxValue = tuple.originMaxValue;
            elementView.SetValue(tuple.normalizedValue);
        }

    }
    private void OnDestroy()
    {
        _audioModel.Release();
        _graphicModel.Release();
        _view.Release();
    }
    

    private void OnChangedViewVisible(bool isEnabled)
    {
        if (isEnabled is false)
        {
            _audioModel.ApplySetting();
            _graphicModel.ApplySetting();
        }
    }

    private void OnChangedModelAudio((string groupKey, float value) tuple)
    {
        if (_view.TryElementView<SliderSettingElementView>("Audio", tuple.groupKey, out var elementView) is false) return;
        
        elementView.SetValue(tuple.value, true);
    }

    private void OnChangedViewAudio((string settingGroupKey, string groupKey, object value) tuple)
    {
        if (tuple.settingGroupKey == "Audio")
        {
            _audioModel.SetVolume(tuple.groupKey, (float)tuple.value);
        }
    }

    private void OnChangedModelGraphic((string graphicGroupKey, object value) tuple)
    {
        if (_view.TryElementView("Graphic", tuple.graphicGroupKey, out DropdownSettingElementView dropdownView))
        {
            dropdownView.SetValue((int)tuple.value, true);
        }
        else if (_view.TryElementView("Graphic", tuple.graphicGroupKey, out SliderSettingElementView sliderView))
        {
            sliderView.SetValue((float)tuple.value, true);
        }
    }

    private void OnChangedViewGraphic((string settingGroupKey, string groupKey, object value) tuple)
    {
        if (tuple.settingGroupKey == "Graphic")
        {
            if (tuple.value is int)
            {
                _graphicModel.SetIndex(tuple.groupKey, (int)tuple.value);
            }
            if (tuple.value is float)
            {
                _graphicModel.SetSliderValue(tuple.groupKey, (float)tuple.value);
            }
        }
    }

}