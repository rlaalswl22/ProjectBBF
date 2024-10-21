using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class RecipeBookPreviewView : MonoBehaviour
{
    [SerializeField] private bool _awakeAndDisable = true;
    
    [SerializeField] private Image _resultItemImage;
    [SerializeField] private TMP_Text _resultItemNameText;
    [SerializeField] private TMP_Text _resultItemDescText;
    
    
    [SerializeField] private Image _bakedBreadResultItemImage;
    [SerializeField] private Image _additiveResultItemImage;

    [SerializeField] private GameObject _additiveFrame;
    
    [SerializeField] private ItemToolTipView _toolTipView;
    
    [SerializeField] private RecipeBookSlotView[] _doughtRecipeItemImages;
    [SerializeField] private RecipeBookSlotView[] _additiveItemImages;

    public bool Visible
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }
    
    public object Data { get; set; }

    private void Awake()
    {
        Visible = !_awakeAndDisable;
        
        Clear();

        foreach (var slotView in _doughtRecipeItemImages)
        {
            slotView.OnHoverEnter += OnHoverEnter;
            slotView.OnHoverExit += OnHoverExit;
            slotView.OnMove += OnMove;
            slotView.OnDown += OnDown;
        }
        foreach (var slotView in _additiveItemImages)
        {
            slotView.OnHoverEnter += OnHoverEnter;
            slotView.OnHoverExit += OnHoverExit;
            slotView.OnMove += OnMove;
            slotView.OnDown += OnDown;
        }
    }

    private void OnDown(object obj)
    {
        if (_toolTipView)
        {
            _toolTipView.Visible = false;
            _toolTipView.Clear();
        }
    }

    private void OnHoverEnter(object obj)
    {
        if (_toolTipView)
        {
            if (SelectItemPresenter.Instance.Model.IsEmpty is false) return;
            if (obj is not ItemData itemData) return;
            _toolTipView.Visible = true;
            _toolTipView.SetText(itemData);
        }
    }

    private void OnHoverExit(object obj)
    {
        if (_toolTipView)
        {
            if (SelectItemPresenter.Instance.Model.IsEmpty is false) return;
            _toolTipView.Visible = false;
            _toolTipView.Clear();
        }
    }

    private void OnMove(object obj, PointerEventData eventData)
    {
        if (_toolTipView)
        {
            var pos = ItemToolTipView.ScreenToOrthogonal(eventData.position);
            pos = _toolTipView.ToValidPosition(pos);
            pos = ItemToolTipView.OrthogonalToScreen(pos);

            _toolTipView.SetPositionWithOffset(pos);
        }
    }

    public void SetView(
        string resultItemName, 
        string resultItemDesc, 
        Sprite resultItemSprite,
        Sprite bakedBreadResultItemSprite,
        Sprite doughResultItemSprite,
        ItemData[] additiveItemSprites,
        ItemData[] doughtItemSprites,
        bool isUnlocked)
    {

        if (isUnlocked)
        {
            _resultItemNameText.text = resultItemName;
            _resultItemDescText.text = resultItemDesc;  
        }
        else
        {
            _resultItemNameText.text = "???";
            _resultItemDescText.text = "???";
        }
        _resultItemImage.sprite = resultItemSprite;

        _bakedBreadResultItemImage.sprite = doughResultItemSprite;
        _additiveResultItemImage.sprite = bakedBreadResultItemSprite;

        Color c = isUnlocked? Color.white : Color.black;
        
        _resultItemImage.color = c;

        if (_doughtRecipeItemImages.Length != doughtItemSprites.Length)
        {
            Debug.LogWarning("반죽 레시피 이미지 슬롯과, 입력된 스프라이트의 수가 일치하지 않습니다.");
        }

        if (additiveItemSprites is not null)
        {
            for (int i = 0; i < Mathf.Min(_additiveItemImages.Length, additiveItemSprites.Length); i++)
            {
                _additiveItemImages[i].SetData(additiveItemSprites[i].ItemSprite, additiveItemSprites[i], true);
            }
            
            _additiveFrame.SetActive(true);
        }
        else
        {
            for (int i = 0; i < _additiveItemImages.Length; i++)
            {
                _additiveItemImages[i].Clear();
            }
            
            _additiveFrame.SetActive(false);
        }
        
        for (int i = 0; i < Mathf.Min(_doughtRecipeItemImages.Length, doughtItemSprites.Length); i++)
        {
            _doughtRecipeItemImages[i].SetData(doughtItemSprites[i].ItemSprite, doughtItemSprites[i], true);
        }
    }

    public void Clear()
    {
        _resultItemNameText.text = "";
        _resultItemDescText.text = "";
        _resultItemImage.sprite = null;

        _bakedBreadResultItemImage.sprite = null;
        _additiveResultItemImage.sprite = null;

        foreach (var t in _additiveItemImages)
        {
            if(t == false)continue;
            t.Clear();
        }

        foreach (var t in _doughtRecipeItemImages)
        {
            if(t == false)continue;
            t.Clear();
        }
    }
}
