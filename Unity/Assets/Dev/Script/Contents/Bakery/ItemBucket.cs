using System;
using DG.Tweening;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemBucket : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _itemRenderer;
    [SerializeField] private SpriteRenderer _bucketRenderer;

    private ItemData _itemData;
    
    public ItemData Item
    {
        get => _itemData;
        set
        {
            _itemData = value;

            _itemRenderer.sprite = value ? value.ItemSprite : null;
        }
    }

    public void OnFade(float t)
    {
        _itemRenderer.SetAlpha(t);
        _bucketRenderer.SetAlpha(t);
    }
}