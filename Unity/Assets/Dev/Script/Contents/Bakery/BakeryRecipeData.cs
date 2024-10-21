using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Bakery/Bakery Recipe Data", fileName = "Dat_Bakery_Recipe_Sample")]
public class BakeryRecipeData : ScriptableObject
{
    [SerializeField, Header("레시피 고유 키(반드시 입력!! 입력 안 하면 해금기능 작동 안 함)")] 
    private string _key;

    [SerializeField, Header("반죽 레시피")] 
    private BakeryDoughRecipeData _doughRecipe;
    
    [FormerlySerializedAs("_breadRecipe")] [SerializeField, Header("구운 빵 레시피")] 
    private BakeryBakingRecipeData _bakingRecipe;

    [SerializeField, Header("연성 재료")] 
    private BakeryAdditiveRecipeData _additiveRecipe;

    [SerializeField, Header("레시피북 설명")]
    private string _description;

    // 최종 결과 아이템이 없으면, 구운 빵아이템 반환하도록 (레시피북에서 구운 빵을 출력하도록 하기위함)
    public ItemData ResultItem
    {
        get
        {
            if (_additiveRecipe)
            {
                return _additiveRecipe.ResultItem;
            }

            return _bakingRecipe.BreadItem;
        }
    }


    public BakeryDoughRecipeData DoughRecipe => _doughRecipe;

    public BakeryBakingRecipeData BakingRecipe => _bakingRecipe;

    public BakeryAdditiveRecipeData AdditiveRecipe => _additiveRecipe;

    public string Description => _description;

    public string Key => _key;
}
