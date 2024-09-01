using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : IInventoryController<GridInventoryModel>
{
    public GridInventoryModel Model { get; private set; }
    private PlayerQuickInventoryView _quickView;
    // TODO: 나중에 메인 inventory view 추가.
    
    public bool QuickInvVisible
    {
        get => _quickView.Visible;
        set => _quickView.Visible = value;
    }
    
    // TODO: 나중에 메인 Inventory visible 추가
    public bool MainInvVisible
    {
        get => _quickView.Visible;
        set => _quickView.Visible = value;
    }

    public PlayerInventoryController(GridInventoryModel model, PlayerQuickInventoryView quickView)
    {
        Model = model;
        _quickView = quickView;
        
        _quickView.Refresh(Model);
    }

    public ItemData CurrentItemData => CurrentItemSlot.Data;
    public IInventorySlot CurrentItemSlot =>  Model.GetSlotSequentially(_quickView.CurrentItemIndex);

    public void Refresh()
    {
        _quickView.Refresh(Model);
        // TODO: 나중에 메인 Inventory refresh 추가. 
    }

    public void HideAll()
    {
        QuickInvVisible = false;
        MainInvVisible = false;
    }
}
