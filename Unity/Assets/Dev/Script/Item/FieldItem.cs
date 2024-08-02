using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;



public class FieldItem : MonoBehaviour
{
    public struct FieldItemInitParameter
    {
        public ItemData ItemData;
        public Vector3 Position;
    }
    
    [NotNull, SerializeField] private ItemData _itemData;
    public ItemData ItemData => _itemData;

    public static FieldItem Create(FieldItemInitParameter parameter)
    {
        var obj = new GameObject("FieldItem_" + parameter.ItemData.ItemName);
        var fieldItem = obj.AddComponent<FieldItem>();
        var renderer = obj.AddComponent<SpriteRenderer>();

       fieldItem._itemData = parameter.ItemData;
       fieldItem.transform.position = parameter.Position;

       renderer.sprite = fieldItem._itemData.ItemSprite;
        
        return fieldItem;
    }
}
