



using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ProjectBBF/Data/Bakery/Baking Recipe Order", fileName = "Dat_Bakery_Recipe_Order")]
public class BakeryRecipeOrderTable : ScriptableObject
{
    [SerializeField] private List<BakeryRecipeData> _orders;


    public List<BakeryRecipeData> Orders => _orders;
}