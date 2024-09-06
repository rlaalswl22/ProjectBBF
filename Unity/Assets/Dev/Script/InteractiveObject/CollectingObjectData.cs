using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/CollectingObjectData", fileName = "CollectingObjectData")]
public class CollectingObjectData : ScriptableObject
{
    [System.Serializable]
    public struct Item
    {
        public ItemData Data;
        public int Count;
    }

    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _collectedSprite;
    [SerializeField] private int _maxCollectCount;
    [SerializeField] private List<Item> _dropItems;

    public Sprite DefaultSprite => _defaultSprite;
    public Sprite CollectedSprite => _collectedSprite;

    public int MaxCollectCount => _maxCollectCount;

    public IReadOnlyList<Item> DropItems => _dropItems;
}
