using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : IInventoryController<GridInventoryModel>
{
    public GridInventoryModel Model { get; private set; }
    private PlayerQuickInventoryView _quickView;
    
    private PlayerMainInventoryView _mainView;
    private PlayerPannelController _pannelController;
    
    public bool QuickInvVisible
    {
        get => _quickView.Visible;
        set => _quickView.Visible = value;
    }
    
    public bool MainInvVisible
    {
        get => _pannelController.ViewState == PlayerPannelController.ViewType.Inv;
        set => _pannelController.ViewState = value ? PlayerPannelController.ViewType.Inv : PlayerPannelController.ViewType.Close;
    }

    public PlayerInventoryController(GridInventoryModel model, PlayerMainInventoryView mainView, PlayerQuickInventoryView quickView, PlayerPannelController pannelController)
    {
        Model = model;
        
        _mainView = mainView;
        _quickView = quickView;
        
        _pannelController = pannelController;
        
        _mainView.Refresh(Model);
        _quickView.Refresh(Model);

        QuickInvVisible = true;
    }

    public ItemData CurrentItemData => CurrentItemSlot?.Data;
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
