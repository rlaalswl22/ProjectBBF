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
    
    [SerializeField] private RecipeBookSlotView _resultItemImage;
    [SerializeField] private TMP_Text _resultItemNameText;
    [SerializeField] private TMP_Text _resultItemDescText;
    
    
    [SerializeField] private RecipeBookSlotView _bakedBreadResultItemImage;
    [SerializeField] private RecipeBookSlotView _additiveResultItemImage;

    [SerializeField] private GameObject _additiveFrame;
    
    [SerializeField] private ItemToolTipView _toolTipView;
    
    [SerializeField] private RecipeBookSlotView[] _doughtRecipeItemImages;
    [SerializeField] private RecipeBookSlotView[] _additiveItemImages;

    public bool Visible
    {
        get => gameObject.activeSelf;
        set
        {
            if(value is false)
                _toolTipView.Visible = false;
            
            gameObject.SetActive(value);
        }
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
        }
        foreach (var slotView in _additiveItemImages)
        {
            slotView.OnHoverEnter += OnHoverEnter;
            slotView.OnHoverExit += OnHoverExit;
            slotView.OnMove += OnMove;
        }
        
        _bakedBreadResultItemImage.OnHoverEnter += OnHoverEnter;
        _bakedBreadResultItemImage.OnHoverExit += OnHoverExit;
        _bakedBreadResultItemImage.OnMove += OnMove;
        
        _additiveResultItemImage.OnHoverEnter += OnHoverEnter;
        _additiveResultItemImage.OnHoverExit += OnHoverExit;
        _additiveResultItemImage.OnMove += OnMove;
    }

    private void OnHoverEnter(RecipeBookSlotView slotView)
    {
        if (_toolTipView)
        {
            if (SelectItemPresenter.Instance.Model.IsEmpty is false) return;
            if (slotView.Data is not ItemData itemData) return;
            _toolTipView.Visible = true;
            _toolTipView.SetText(itemData);
        }
    }

    private void OnHoverExit(RecipeBookSlotView slotView)
    {
        if (_toolTipView)
        {
            if (SelectItemPresenter.Instance.Model.IsEmpty is false) return;
            _toolTipView.Visible = false;
            _toolTipView.Clear();
        }
    }

    private void OnMove(RecipeBookSlotView slotView, PointerEventData eventData)
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
        ItemData resultItemSprite,
        ItemData bakedBreadResultItemSprite,
        ItemData doughResultItemSprite,
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
        
        // 각 단계 대표 이미지들
        _resultItemImage.SetData(resultItemSprite.ItemSprite, resultItemSprite, isUnlocked);
        _bakedBreadResultItemImage.SetData(doughResultItemSprite.ItemSprite, doughResultItemSprite, true);
        _additiveResultItemImage.SetData(bakedBreadResultItemSprite.ItemSprite, bakedBreadResultItemSprite, true);
        

        // 연성 재료 칸
        {
            foreach (RecipeBookSlotView slot in _additiveItemImages)
            {
                slot.Clear();
            }
            
            if (additiveItemSprites is not null)
            {
                for (int i = 0; i < Mathf.Min(_additiveItemImages.Length, additiveItemSprites.Length); i++)
                {
                    _additiveItemImages[i].SetData(additiveItemSprites[i].ItemSprite, additiveItemSprites[i], true);
                }
            }
        
            _additiveFrame.SetActive(additiveItemSprites is not null);
        }


        // 반죽 재료 칸
        {
            if (_doughtRecipeItemImages.Length != doughtItemSprites.Length)
            {
                Debug.LogWarning("반죽 레시피 이미지 슬롯과, 입력된 스프라이트의 수가 일치하지 않습니다.");
            }
        
            for (int i = 0; i < Mathf.Min(_doughtRecipeItemImages.Length, doughtItemSprites.Length); i++)
            {
                _doughtRecipeItemImages[i].SetData(doughtItemSprites[i].ItemSprite, doughtItemSprites[i], true);
            }
        }
    }

    public void Clear()
    {
        _resultItemNameText.text = "";
        _resultItemDescText.text = "";
        _resultItemImage.Clear();

        _bakedBreadResultItemImage.Clear();
        _additiveResultItemImage.Clear();

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
