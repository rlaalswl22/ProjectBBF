using System;
using System.Collections.Generic;
using System.Linq;
using ProjectBBF.Singleton;
using UnityEngine;

[Singleton(ESingletonType.Global, initializeOrder: 5)]
public class BakeryRecipeResolver: MonoBehaviourSingleton<BakeryRecipeResolver>
{
    private const string FAIL_RECIPE_DOUGH = "Data/Bakery/Reservation/Dat_Barkey_Recipe_DoughFail";
    private const string FAIL_RECIPE_BAKED_BREAD = "Data/Bakery/Reservation/Dat_Barkey_Recipe_BakedFail";
    private const string FAIL_RECIPE_RESULT_BREAD = "Data/Bakery/Reservation/Dat_Barkey_Recipe_AdditiveFail";
    private const string DOUGH_INGREDIENT_TABLE = "Data/Bakery/Table/Dat_Bakery_Table_DoughIngredient";
    private const string DOUGH_TABLE = "Data/Bakery/Table/Dat_Bakery_Table_Dough";
    private const string BAKED_BREAD_TABLE = "Data/Bakery/Table/Dat_Bakery_Table_BakedBread";
    private const string ADDITIVE_INGREDIENT_TABLE = "Data/Bakery/Table/Dat_Bakery_Table_Additive";
    private const string COMPLETION_BREAD_TABLE = "Data/Bakery/Table/Dat_Bakery_Table_Completion";
    private const string ORDER_TABLE = "Data/Bakery/Table/Dat_Bakery_Recipe_Order";
    
    private Dictionary<string, BakeryRecipeData> _recipeTable;
    
    public IReadOnlyDictionary<string, BakeryRecipeData> RecipeTable => _recipeTable;
    
    public BakeryDoughRecipeData FailDoughRecipe { get; private set; }
    public BakeryBakingRecipeData FailBakedBreadRecipe { get; private set; }
    public BakeryAdditiveRecipeData FailResultBreadRecipe { get; private set; }
    public BakeryIngredientTableData DoughIngredientTable { get; private set; }
    public BakeryIngredientTableData DoughTable { get; private set; }
    public BakeryIngredientTableData AdditiveIngredientTable { get; private set; }
    public BakeryIngredientTableData BakedBreadTable { get; private set; }
    public BakeryIngredientTableData CompletionBreadTable { get; private set; }
    
    public BakeryRecipeOrderTable OrderTable { get; private set; }

    private HashSet<ItemData> _doughIngredientHashSet;
    private HashSet<ItemData> _doughHashSet;
    private HashSet<ItemData> _additiveIngredientHashSet;
    private HashSet<ItemData> _bakedBreadHashSet;
    private HashSet<ItemData> _completionBreadHashSet;
    
    public override void PostInitialize()
    {
        _recipeTable = new();
        
        FailDoughRecipe = Resources.Load<BakeryDoughRecipeData>(FAIL_RECIPE_DOUGH);
        FailBakedBreadRecipe = Resources.Load<BakeryBakingRecipeData>(FAIL_RECIPE_BAKED_BREAD);
        FailResultBreadRecipe = Resources.Load<BakeryAdditiveRecipeData>(FAIL_RECIPE_RESULT_BREAD);
        
        DoughIngredientTable = Resources.Load<BakeryIngredientTableData>(DOUGH_INGREDIENT_TABLE);
        DoughTable = Resources.Load<BakeryIngredientTableData>(DOUGH_TABLE);
        AdditiveIngredientTable = Resources.Load<BakeryIngredientTableData>(ADDITIVE_INGREDIENT_TABLE);
        BakedBreadTable = Resources.Load<BakeryIngredientTableData>(BAKED_BREAD_TABLE);
        CompletionBreadTable = Resources.Load<BakeryIngredientTableData>(COMPLETION_BREAD_TABLE);
        
        OrderTable = Resources.Load<BakeryRecipeOrderTable>(ORDER_TABLE);

        _doughIngredientHashSet = new(DoughIngredientTable.Ingredients);
        _doughHashSet = new(DoughTable.Ingredients);
        _additiveIngredientHashSet = new(AdditiveIngredientTable.Ingredients);
        _bakedBreadHashSet = new(BakedBreadTable.Ingredients);
        _completionBreadHashSet = new(CompletionBreadTable.Ingredients);
        
        foreach (BakeryRecipeData data in OrderTable.Orders)
        {
            _recipeTable.Add(data.Key, data);
        }
    }
    
    public override void PostRelease()
    {
        _recipeTable?.Clear();
        _recipeTable = null;
    }

    public bool CanListOnDoughIngredient(ItemData doughIngredient)
        => _doughIngredientHashSet.Contains(doughIngredient);
    public bool CanListOnDough(ItemData dough)
        => _doughHashSet.Contains(dough);
    public bool CanListOnBakedBread(ItemData bakedBread)
        => _bakedBreadHashSet.Contains(bakedBread);
    public bool CanListOnAdditive(ItemData additiveItem)
        => _additiveIngredientHashSet.Contains(additiveItem);
    public bool CanListOnCompletionBread(ItemData bread)
        => _completionBreadHashSet.Contains(bread);

    public BakeryDoughRecipeData ResolveDough(IReadOnlyList<ItemData> ingredients)
    {
        foreach (var recipe in RecipeTable.Values)
        {
            int count = 0;
            foreach (var item in ingredients)
            {
                count += recipe.DoughRecipe.Ingredients.Contains(item) ? 1 : 0;
            }
            if (ingredients.Count == count)
            {
                return recipe.DoughRecipe;
            }
        }

        return null;
    }
    
    public BakeryBakingRecipeData ResolveBakedBread(ItemData dought, out BakeryRecipeData bakedBreadRecipeData)
    {
        bakedBreadRecipeData = null;
        BakeryBakingRecipeData result = null;
        foreach (var recipe in RecipeTable.Values)
        {
            if (recipe.BakingRecipe.DoughtItem == dought && recipe.AdditiveRecipe == null)
            {
                bakedBreadRecipeData = recipe;
                result = recipe.BakingRecipe;
                break;
            }
        }

        if (result == null) return null;

        if (bakedBreadRecipeData)
        {
            return result;
        }

        return null;
    }
    
    public BakeryAdditiveRecipeData ResolveAdditive(ItemData bakedBread, IReadOnlyList<ItemData> additives, out BakeryRecipeData recipeData)
    {
        foreach (var recipe in RecipeTable.Values)
        {
            foreach (ItemData item in additives)
            {
                if (recipe.AdditiveRecipe.AdditiveItems.Contains(item) && bakedBread == recipe.AdditiveRecipe.BreadItem)
                {
                    recipeData = recipe;
                    return recipe.AdditiveRecipe;
                }
            }
        }

        recipeData = null;
        return null;
    }
}