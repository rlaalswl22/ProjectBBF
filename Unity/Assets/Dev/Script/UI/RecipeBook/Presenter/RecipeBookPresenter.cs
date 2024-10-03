using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeBookPresenter : MonoBehaviour
{
    [SerializeField] private RecipeBookListView _listView;
    [SerializeField] private RecipeBookPreviewView _previewView;

    public RecipeBookListView ListView => _listView;

    public RecipeBookPreviewView PreviewView => _previewView;

    public RecipeBookModel Model { get; private set; }

    public void Awake()
    {
        var resolver = BakeryRecipeResolver.Instance;
        Model = resolver.Model;
        
        _listView.OnSlotClick += OnSlotClicked;
        Model.OnRecipeUnlocked += OnUnlockedChanged;


        foreach (var data in resolver.RecipeTable.Values)
        {
            _listView.AddItem(data.ResultItem.ItemSprite, data, Model.IsUnlocked(data.Key));
        }
    }

    private void OnUnlockedChanged(string recipeKey)
    {
        var recipeBookSlotView = _listView.Slots.Where(x =>
        {
            if (x.Data is BakeryRecipeData recipe)
            {
                return recipe.Key == recipeKey;
            }

            return false;
        }).FirstOrDefault();

        if (recipeBookSlotView == false) return;
        
        recipeBookSlotView.SetUnlocked(true);

        if (BakeryRecipeResolver.Instance.RecipeTable.TryGetValue(recipeKey, out var r))
        {
            if (_previewView.Data is BakeryRecipeData d && d  == r)
            {
                OnSlotClicked(_previewView.Data);
            }
        }
    }

    private void OnDestroy()
    {
        _listView.OnSlotClick -= OnSlotClicked;
        Model.OnRecipeUnlocked -= OnUnlockedChanged;
    }

    private void OnSlotClicked(object data)
    {
        if (data is not BakeryRecipeData recipe) return;
        
        bool isUnlocked = Model.IsUnlocked(recipe.Key);

        _previewView.SetView(
            recipe.ResultItem.ItemName,
            recipe.ResultItem.ItemDescription,
            recipe.ResultItem.ItemSprite,
            recipe.AdditiveRecipe.AdditiveItems.Select(x => x.ItemSprite).ToArray(),
            recipe.DoughRecipe.Ingredients.Select(x => x.ItemSprite).ToArray(),
            isUnlocked
        );

        _previewView.Data = recipe;
    }
}
