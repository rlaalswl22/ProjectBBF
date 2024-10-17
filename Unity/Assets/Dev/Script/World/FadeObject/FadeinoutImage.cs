using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FadeinoutObject))]
public class FadeinoutImage : MonoBehaviour
{
    [SerializeField] private List<Image> _targetRenderers;

    FadeinoutObject _fadeObject;
    
    private void Awake()
    {
        _fadeObject = GetComponent<FadeinoutObject>();
    }

    private void OnFade(float obj)
    {
        foreach (Image image in _targetRenderers)
        {
            if (image)
            {
                image.SetAlpha(obj);
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