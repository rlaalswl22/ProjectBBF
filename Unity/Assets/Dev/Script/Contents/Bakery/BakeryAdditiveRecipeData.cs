using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Bakery/Additive Recipe Data", fileName = "Dat_Bakery_Add_Sample")]
public class BakeryAdditiveRecipeData : ScriptableObject
{
    [SerializeField, Header("구운 빵 아이템")] 
    private ItemData _breadItem;
    [SerializeField, Header("연성 재료 아이템 0")] 
    private ItemData _additiveItem0;
    [SerializeField, Header("연성 재료 아이템 1")] 
    private ItemData _additiveItem1;
    [SerializeField, Header("최종 결과 아이템")] 
    private ItemData _resultItem;
    
    [SerializeField, Header("제작 시간")] 
    private float _completionDuration;

    public ItemData BreadItem => _breadItem;
    public ItemData ResultItem => _resultItem;

    public ItemData[] AdditiveItems
    {
        get
        {
            if (_additiveItem1)
            {
                return new ItemData[]
                {
                    _additiveItem0,
                    _additiveItem1,
                };
            }
            
            return new ItemData[]
            {
                _additiveItem0,
            };
        }
    }

    public float CompletionDuration => _completionDuration;
}