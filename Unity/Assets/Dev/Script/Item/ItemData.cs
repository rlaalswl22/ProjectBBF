using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ProjectBBF/ItemData", fileName = "NewItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] protected string _itemName;
    [SerializeField] protected Sprite _itemSprite;
    [SerializeField] protected float _lootingTime;
    [SerializeField] protected string _itemDescription;
    
    public string ItemName => _itemName;
    public Sprite ItemSprite => _itemSprite;
    public float LootingTime => _lootingTime;
    public string ItemDescription => _itemDescription;
}
