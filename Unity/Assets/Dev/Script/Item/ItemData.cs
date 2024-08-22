using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "ProjectBBF/Data/Item/Default item", fileName = "New Default item data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemSprite;
    [SerializeField] private float _lootingTime;
    [SerializeField] private string _itemDescription;
    [SerializeField] private int _maxStackCount = 1;
    [SerializeField] private ItemTypeInfo _itemTypeInfo;
    [SerializeField] private ActionCategoryType _actionCategoryType;
    [SerializeField] private ActionAnimationType _actionAnimationType;
    
    public string ItemName => _itemName;
    public Sprite ItemSprite => _itemSprite;
    public float LootingTime => _lootingTime;
    public string ItemDescription => _itemDescription;
    public int MaxStackCount => _maxStackCount;
    public ItemTypeInfo Info => _itemTypeInfo;

    public ActionCategoryType ActionCategoryType => _actionCategoryType;

    public ActionAnimationType ActionAnimationType => _actionAnimationType;
}
