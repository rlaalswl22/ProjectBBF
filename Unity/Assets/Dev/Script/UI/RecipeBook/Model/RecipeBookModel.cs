using System;
using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using UnityEngine;

public class RecipeBookModel
{
    public event Action<string> OnRecipeUnlocked; 
        
    private readonly RecipeBookPersistenceObject _saveData;
    

    public void Add(string recipeKey)
    {
        _saveData.UnlockRecipeKeys.Add(recipeKey);
        OnRecipeUnlocked?.Invoke(recipeKey);
    }
    
    public bool IsUnlocked(string key)
    {
        return _saveData.UnlockRecipeKeys.Contains(key);
    }
    
    public RecipeBookModel()
    {
        _saveData = PersistenceManager.Instance.LoadOrCreate<RecipeBookPersistenceObject>("RecipeBook");
    }
    
    
}
