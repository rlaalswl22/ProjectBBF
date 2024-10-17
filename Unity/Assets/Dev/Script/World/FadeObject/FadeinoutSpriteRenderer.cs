using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[RequireComponent(typeof(FadeinoutObject))]
public class FadeinoutSpriteRenderer : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> _targetRenderers;

    FadeinoutObject _fadeObject;
    
    private void Awake()
    {
        _fadeObject = GetComponent<FadeinoutObject>();
    }

    private void OnFade(float obj)
    {
        foreach (SpriteRenderer spriteRenderer in _targetRenderers)
        {
            if (spriteRenderer)
            {
                spriteRenderer.SetAlpha(obj);
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