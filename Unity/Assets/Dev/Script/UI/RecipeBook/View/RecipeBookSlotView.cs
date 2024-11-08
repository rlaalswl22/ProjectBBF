using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeBookSlotView : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private RecipeBookMarkerView _markerView;
    
    private Button _button;
    
    public object Data { get; set; }

    public event Action<RecipeBookSlotView> OnClick; 
    public event Action<RecipeBookSlotView> OnDown;
    public event Action<RecipeBookSlotView> OnHoverEnter;
    public event Action<RecipeBookSlotView> OnHoverExit;
    public event Action<RecipeBookSlotView, PointerEventData> OnMove;
    public Sprite Sprite
    {
        get => _itemImage.sprite;
        set => _itemImage.sprite = value;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        if (_markerView)
        {
            _markerView.gameObject.SetActive(false);
        }

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
        UpdateBookmark(data);
        
        Data = data;
        Sprite = sprite;

        SetUnlocked(isUnlocked);
    }

    public void UpdateBookmark(object data)
    {
        if (_markerView)
        {
            bool isSameData = data == Data;
            _markerView.IsBookmarked = isSameData;
            _markerView.gameObject.SetActive(isSameData);
        }
    }

    public void SetUnlocked(bool isUnlocked)
    {
        // unlocked 기능은 deprecated됨
        isUnlocked = true;
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
        AudioManager.Instance.PlayOneShot("UI", "UI_MouseOver");
        OnDown?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShot("UI", "UI_MouseOver");
        OnHoverEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit?.Invoke(this);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        OnMove?.Invoke(this, eventData);
    }
}
