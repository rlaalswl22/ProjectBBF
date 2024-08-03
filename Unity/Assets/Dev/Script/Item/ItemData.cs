using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ProjectBBF/ItemData", fileName = "NewItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemSprite;
    [SerializeField] private float _lootingTime;
    [SerializeField] private string _itemDescription;
    
    public string ItemName => _itemName;
    public Sprite ItemSprite => _itemSprite;
    public float LootingTime => _lootingTime;
    public string ItemDescription => _itemDescription;
}
