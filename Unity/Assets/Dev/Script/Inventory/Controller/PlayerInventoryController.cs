using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : IInventoryController<GridInventoryModel>
{
    public GridInventoryModel Model { get; private set; }
    private PlayerQuickInventoryView _quickView;
    private PlayerMainInventoryView _mainView;
    
    public bool QuickInvVisible
    {
        get => _quickView.Visible;
        set => _quickView.Visible = value;
    }
    
    public bool MainInvVisible
    {
        get => _mainView.Visible;
        set => _mainView.Visible = value;
    }

    public PlayerInventoryController(GridInventoryModel model, PlayerMainInventoryView mainView, PlayerQuickInventoryView quickView)
    {
        Model = model;
        
        _mainView = mainView;
        _quickView = quickView;
        
        _mainView.Refresh(Model);
        _quickView.Refresh(Model);

        QuickInvVisible = true;
        MainInvVisible = false;
    }

    public ItemData CurrentItemData => CurrentItemSlot.Data;
    public IInventorySlot CurrentItemSlot =>  Model.GetSlotSequentially(_quickView.CurrentItemIndex);

    public void Refresh()
    {
        _mainView.Refresh(Model);
        _quickView.Refresh(Model);
    }

    public void HideAll()
    {
        MainInvVisible = false;
        QuickInvVisible = false;
    }
}
