using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Bakery/Baking Recipe Data", fileName = "Dat_Bakery_Baking_Sample")]
public class BakeryBakingRecipeData : ScriptableObject
{
    [SerializeField, Header("반죽 아이템")] 
    private ItemData _doughtItem;
    [SerializeField, Header("구운 빵 아이템")] 
    private ItemData _breadItem;
    [SerializeField, Header("미니게임 바 시간")] 
    private float _minigameBarDuration;

    [SerializeField, Header("레시피북 설명")]
    private string _description;
    
    public ItemData DoughtItem => _doughtItem;
    public ItemData BreadItem => _breadItem;

    public float MinigameBarDuration => _minigameBarDuration;
    public string Description => _description;
}