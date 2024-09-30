using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeBookPresenter : MonoBehaviour
{
    [SerializeField] private RecipeBookListView _listView;
    [SerializeField] private RecipeBookPreviewView _previewView;

    private List<BakeryRecipeData> _recipes;

    private const string RECIPES_PATH = "Data/Recipe/";

    public RecipeBookListView ListView => _listView;

    public RecipeBookPreviewView PreviewView => _previewView;

    public RecipeBookModel Model { get; private set; }

    public void Awake()
    {
        Model = new();
        
        _listView.OnSlotClick += OnSlotClicked;
        Model.OnRecipeUnlocked += OnUnlockedChanged;

        var arr = Resources.LoadAll<BakeryRecipeData>(RECIPES_PATH);
        _recipes = new(arr);

        foreach (var data in _recipes)
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
        
        recipeBookSlotView?.SetUnlocked(true);
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
            recipe.AdditiveItem.Select(x => x.ItemSprite).ToArray(),
            recipe.DoughtItems.Select(x => x.ItemSprite).ToArray(),
            isUnlocked
        );
    }
}
