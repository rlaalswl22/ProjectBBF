using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorInventory : IInventory
{
    private readonly IInventory _masterInventory;
    public readonly int MirrorItemCount;
    private IInventoryView _view;
    
    public MirrorInventory(IInventory masterInventory, IInventoryView view, int mirrorItemCount)
    {
        Debug.Assert(masterInventory is not MirrorInventory);
        
        _masterInventory = masterInventory;
        MirrorItemCount = mirrorItemCount;
        _view = view;
    }

    public int MaxSize => _masterInventory.MaxSize;
    public IInventorySlot GetSlotSequentially(int index) => _masterInventory.GetSlotSequentially(index);
    public void ViewRefresh()
    {
        _view.Refresh();
    }
}
