using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuickInventoryController : MonoBehaviour, IInventory
{
    [SerializeField] private PlayerQuickInventoryView _view;
    
    private PlayerController _playerController;
    private MirrorInventory _inventory;
    
    public bool Visible
    {
        get => _view.Visible;
        set => _view.Visible = value;
    }

    public void Init(PlayerController controller)
    {
        _playerController = controller;
        _inventory = new MirrorInventory(_playerController.Inventory, _view, _view.MaxSlotCount);
        _view.Init(_inventory);
    }

    public int MaxSize { get; }
    public IInventorySlot GetSlotSequentially(int index)
    {
        return _inventory.GetSlotSequentially(index);
    }

    public void ViewRefresh()
    {
        _view.Refresh();
    }

    public ItemData CurrentItemData => CurrentItemSlot.Data;
    public IInventorySlot CurrentItemSlot =>  _inventory.GetSlotSequentially(_view.CurrentItemIndex);
}
