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
    
    [SerializeField, Header("최종 결과 아이템")] 
    private ItemData _resultItem;
    
    public ItemData ResultItem => _resultItem;


    public BakeryDoughRecipeData DoughRecipe => _doughRecipe;

    public BakeryBakingRecipeData BakingRecipe => _bakingRecipe;

    public BakeryAdditiveRecipeData AdditiveRecipe => _additiveRecipe;
    public string Key => _key;
}
