using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScrollbarSizeSetter : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private float _size;
    
    private void Awake()
    {
        Invoke("InvokeOnScroll", 0.02f);
        _scrollRect.onValueChanged.AddListener(OnScroll);

        InvokeOnScroll();
    }

    private void OnEnable()
    {
        Invoke("InvokeOnScroll", 0.02f);
    }
    
    private void OnDestroy()
    {
        if (IsInvoking("InvokeOnScroll"))
        {
            CancelInvoke("InvokeOnScroll");
        }
        _scrollRect.onValueChanged.RemoveListener(OnScroll);
    }

    private void InvokeOnScroll()
    {
        //_scrollbar.value = 1f;
        OnScroll(default);
    }
    
    private void OnScroll(Vector2 _)
    {
        _scrollbar.size = _size;
    }
}
