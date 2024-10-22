using System;
using System.Collections.Generic;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FadeinoutObject))]
public class FadeinoutImage : MonoBehaviour
{
    [SerializeField] private List<Image> _targetRenderers;
    [SerializeField] private List<TMP_Text> _targetTexts;

    FadeinoutObject _fadeObject;
    
    private void Awake()
    {
        _fadeObject = GetComponent<FadeinoutObject>();
        
        OnFade(0f);
    }

    private void OnFade(float obj)
    {
        foreach (var target in _targetRenderers)
        {
            if (target)
            {
                target.SetAlpha(obj);
            }
        }
        foreach (var target in _targetTexts)
        {
            if (target)
            {
                target.SetAlpha(obj);
            }
        }
    }

    private void OnEnable()
    {
        if (_fadeObject)
        {
            _fadeObject.OnFadeAlpha += OnFade;
        }
    }

    private void OnDisable()
    {
        if (_fadeObject)
        {
            _fadeObject.OnFadeAlpha -= OnFade;
        }
    }
}