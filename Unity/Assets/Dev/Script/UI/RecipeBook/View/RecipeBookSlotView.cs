using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeBookSlotView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _itemImage;
    
    private Button _button;
    
    public object Data { get; set; }

    public event Action<RecipeBookSlotView> OnClick; 
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
}
