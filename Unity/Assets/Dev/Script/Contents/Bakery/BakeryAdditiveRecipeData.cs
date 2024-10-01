using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Bakery/Additive Recipe Data", fileName = "Dat_Bakery_Add_Sample")]
public class BakeryAdditiveRecipeData : ScriptableObject
{
    [SerializeField, Header("구운 빵 아이템")] 
    private ItemData _breadItem;
    [SerializeField, Header("연성 재료 아이템")] 
    private ItemData _additiveItem0;
    [SerializeField, Header("제작 시간")] 
    private float _completionDuration;

    public ItemData BreadItem => _breadItem;

    public ItemData[] AdditiveItems => new ItemData[]
    {
        _additiveItem0
    };

    public float CompletionDuration => _completionDuration;
}