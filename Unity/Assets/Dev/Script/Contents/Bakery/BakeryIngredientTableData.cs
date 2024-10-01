using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Bakery/Ingredient Table data", fileName = "Dat_Bakery_Table_Sample")]
public class BakeryIngredientTableData : ScriptableObject
{
    [SerializeField, Header("재료 테이블")] 
    private List<ItemData> _ingredients;

    public IReadOnlyList<ItemData> Ingredients => _ingredients;
}