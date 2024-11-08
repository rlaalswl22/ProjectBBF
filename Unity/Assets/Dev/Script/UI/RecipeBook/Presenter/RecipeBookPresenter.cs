using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookPresenter : MonoBehaviour
{
    [SerializeField] private RecipeBookMarkerView _bookMark;
    [SerializeField] private BakeryRecipeData _firstSelectedRecipe;
    [SerializeField] private RecipeBookListView _listView;
    [SerializeField] private RecipeBookPreviewView _previewView;
    [SerializeField] private RecipeBookPreviewView _previewSummaryView;

    public RecipeBookListView ListView => _listView;

    public RecipeBookPreviewView PreviewView => _previewView;
    public RecipeBookPreviewView PreviewSummaryView => _previewSummaryView;

    public RecipeBookModel Model { get; private set; }

    public BakeryRecipeData CurrentRecipe
    {
        get => _currentRecipe;
        set
        {
            _prevRecipe = _currentRecipe;
            _currentRecipe = value;
        }
    }

    public BakeryRecipeData PrevRecipe => _prevRecipe;
    
    private BakeryRecipeData _prevRecipe;
    private BakeryRecipeData _currentRecipe;

    public void Awake()
    {
        var resolver = BakeryRecipeResolver.Instance;
        Model = new();
        
        _listView.OnSlotClick += OnSlotClicked;
        Model.OnRecipeUnlocked += OnUnlockedChanged;

        _bookMark.Button.onClick.AddListener(() =>
        {
            _previewSummaryView.Clear();
            
            if (CurrentRecipe == PrevRecipe && _previewSummaryView.Visible)
            {
                _previewSummaryView.Visible = false;
                _bookMark.IsBookmarked = false;
                _listView.UpdateBookmark(null);
            }
            else
            {
                if (CurrentRecipe)
                {
                    SetView(CurrentRecipe, _previewSummaryView);
                    _previewSummaryView.Data = CurrentRecipe;
                    _previewSummaryView.Visible = true;
                    _bookMark.IsBookmarked = true;
                    _listView.UpdateBookmark(CurrentRecipe);
                }
            }
            
        
        });

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
            CurrentRecipe = _firstSelectedRecipe;
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

        SetView(recipe, _previewView);

        bool isSameData = data == _previewSummaryView.Data;
        _bookMark.IsBookmarked = isSameData;
        if (isSameData)
        {
            SetView(_firstSelectedRecipe, _previewSummaryView);
        }

        _previewView.Data = recipe;
    }

    private void SetView(BakeryRecipeData recipe, RecipeBookPreviewView view)
    {
        bool isUnlocked = Model.IsUnlocked(recipe.Key);

        CurrentRecipe = recipe;
        
        view.SetView(
            recipe.ResultItem.ItemName,
            recipe.Description,
            recipe.ResultItem,
            recipe.BakingRecipe.BreadItem,
            recipe.BakingRecipe.DoughtItem,
            recipe.AdditiveRecipe?.AdditiveItems.Select(x => x).ToArray(),
            recipe.DoughRecipe.Ingredients.Select(x => x).ToArray(),
            isUnlocked
        );
    }
}
