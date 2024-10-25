using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;

public class RecipeBookPresenter : MonoBehaviour
{
    [SerializeField] private BakeryRecipeData _firstSelectedRecipe;
    [SerializeField] private RecipeBookListView _listView;
    [SerializeField] private RecipeBookPreviewView _previewView;

    public RecipeBookListView ListView => _listView;

    public RecipeBookPreviewView PreviewView => _previewView;

    public RecipeBookModel Model { get; private set; }

    public void Awake()
    {
        var resolver = BakeryRecipeResolver.Instance;
        Model = new();
        
        _listView.OnSlotClick += OnSlotClicked;
        Model.OnRecipeUnlocked += OnUnlockedChanged;

        _listView.OnExit += () =>
        {
            if (GameObjectStorage.Instance.TryGetPlayerController(out var pc))
            {
                pc.StateHandler.TranslateState("EndOfUIInteraction");
            }
        };

        foreach (var data in resolver.RecipeTable.Values)
        {
            bool isUnlock = Model.IsUnlocked(data.Key);
            if (isUnlock is false && data.InitUnlock)
            {
                isUnlock = true;
                Model.Add(data.Key);
            }
            
            _listView.AddItem(data.ResultItem.ItemSprite, data, isUnlock);
        }

        if (_firstSelectedRecipe)
        {
            BakeryRecipeResolver.Instance.RecipeTable.ForEach(x =>
            {
                if (x.Key == _firstSelectedRecipe.Key)
                {
                    OnSlotClicked(_firstSelectedRecipe);
                }

            });
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
            recipe.Description,
            recipe.ResultItem,
            recipe.BakingRecipe.BreadItem,
            recipe.BakingRecipe.DoughtItem,
            recipe.AdditiveRecipe?.AdditiveItems.Select(x => x).ToArray(),
            recipe.DoughRecipe.Ingredients.Select(x => x).ToArray(),
            isUnlocked
        );

        _previewView.Data = recipe;
    }
}
