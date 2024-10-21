using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeBookSlotView : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private Image _itemImage;
    
    private Button _button;
    
    public object Data { get; set; }

    public event Action<RecipeBookSlotView> OnClick; 
    public event Action<object> OnDown;
    public event Action<object> OnHoverEnter;
    public event Action<object> OnHoverExit;
    public event Action<object, PointerEventData> OnMove;
    public Sprite Sprite
    {
        get => _itemImage.sprite;
        set => _itemImage.sprite = value;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (_button == false)
        {
            _button = GetComponentInChildren<Button>();
        }
        
        if (_button)
        {
            _button.onClick.AddListener(() => OnPointerClick(null));
        }
    }

    public void SetData(Sprite sprite, object data, bool isUnlocked)
    {
        Data = data;
        Sprite = sprite;

        SetUnlocked(isUnlocked);
    }

    public void SetUnlocked(bool isUnlocked)
    {
        _itemImage.color = isUnlocked ? Color.white : Color.black;
    }

    public void Clear()
    {
        Data = null;
        Sprite = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }

    
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown?.Invoke(Data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter?.Invoke(Data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit?.Invoke(Data);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        OnMove?.Invoke(Data, eventData);
    }
}
