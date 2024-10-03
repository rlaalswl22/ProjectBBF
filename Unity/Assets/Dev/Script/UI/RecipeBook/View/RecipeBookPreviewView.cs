using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class RecipeBookPreviewView : MonoBehaviour
{
    [SerializeField] private bool _awakeAndDisable = true;
    
    [SerializeField] private Image _resultItemImage;
    [SerializeField] private TMP_Text _resultItemNameText;
    [SerializeField] private TMP_Text _resultItemDescText;
    
    [SerializeField] private Image[] _doughtRecipeItemImages;
    [SerializeField] private Image[] _additiveItemImages;

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
    }

    public void SetView(
        string resultItemName, 
        string resultItemDesc, 
        Sprite resultItemSprite,
        Sprite[] additiveItemSprites,
        Sprite[] doughtItemSprites,
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

        Color c = isUnlocked? Color.white : Color.black;
        
        _resultItemImage.color = c;

        if (_doughtRecipeItemImages.Length != doughtItemSprites.Length)
        {
            Debug.LogWarning("반죽 레시피 이미지 슬롯과, 입력된 스프라이트의 수가 일치하지 않습니다.");
        }
        
        for (int i = 0; i < Mathf.Min(_additiveItemImages.Length, additiveItemSprites.Length); i++)
        {
            _additiveItemImages[i].sprite = additiveItemSprites[i];
        }
        for (int i = 0; i < Mathf.Min(_doughtRecipeItemImages.Length, doughtItemSprites.Length); i++)
        {
            _doughtRecipeItemImages[i].sprite = doughtItemSprites[i];
        }
    }

    public void Clear()
    {
        _resultItemNameText.text = "";
        _resultItemDescText.text = "";
        _resultItemImage.sprite = null;

        foreach (var t in _additiveItemImages)
        {
            t.sprite = null;
        }

        foreach (var t in _doughtRecipeItemImages)
        {
            t.sprite = null;
        }
    }
}
