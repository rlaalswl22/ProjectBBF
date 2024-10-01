using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Bakery/Dough Recipe Data", fileName = "Dat_Bakery_Dough_Sample")]
public class BakeryDoughRecipeData : ScriptableObject
{
    [SerializeField, Header("최대 3개의 재료 가능")]
    private List<ItemData> _ingredients;

    [SerializeField, Header("반죽 아이템")] 
    private ItemData _doughItem;

    [SerializeField, Header("반죽 시간")] 
    private float _kneadDuration;
    
    public IReadOnlyList<ItemData> Ingredients => _ingredients;
    public ItemData DoughItem => _doughItem;

    public float KneadDuration => _kneadDuration;

    public const int MAX_INGREDIENT_COUNT = 3;

    private void OnValidate()
    {
        if (_ingredients is null) return;
        if (_ingredients.Count > MAX_INGREDIENT_COUNT)
        {
            _ingredients.RemoveRange(MAX_INGREDIENT_COUNT, _ingredients.Count - MAX_INGREDIENT_COUNT);
        }
    }
}