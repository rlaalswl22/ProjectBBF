using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventorySlot
{
    public event Action<IInventorySlot> Chnaged;
    public ItemData Data { get; }
    public int Count { get; }
    public bool Empty { get; }

    public bool TrySetCount(int value, bool allowZeroEmpty = false);
    public bool TrySet(ItemData itemData, int count, InventorySlotSetMethod method);
    public bool CanSet(ItemData itemData, int count, InventorySlotSetMethod method);
    public void Swap(IInventorySlot slot);
    public void Clear();
}

public interface IInventory
{
    public int MaxSize { get; }
    public IInventorySlot GetSlotSequentially(int index);
    public void ViewRefresh();
}

public interface IInventoryView
{
    public void Refresh();
}

public enum InventorySlotSetMethod
{
    Set,
    Add
}

public class InventorySlot : IInventorySlot
{
    public event Action<IInventorySlot> Chnaged;
    public ItemData Data { get; private set; }
    public int Count { get; private set; }
    
    public bool Empty => Data is null;

    public bool TrySetCount(int value, bool allowZeroEmpty = false)
    {
        if (Data is null) return false;
        if (Data.MaxStackCount <= value) return false;
        if (value < 1 && allowZeroEmpty == false) return false;
        
        Count = value;

        if (allowZeroEmpty && Count < 1)
        {
            Data = null;
            Count = 0;
        }
        
        Chnaged?.Invoke(this);
        
        return true;
    }

    public bool CanSet(ItemData itemData, int count, InventorySlotSetMethod method)
    {
        if (itemData is null)
        {
            return true;
        }

        if (method == InventorySlotSetMethod.Set)
        {
            if (count < 1 || count > itemData.MaxStackCount)
            {
                return false;
            }
        }
        
        if (method == InventorySlotSetMethod.Add)
        {
            if (count + this.Count > itemData.MaxStackCount || count + this.Count < 1)
            {
                return false;
            }
        }

        return true;
    }

    public void Swap(IInventorySlot slot)
    {
        var data = slot.Data;
        var count = slot.Count;

        var myData = Data;
        var myCount = Count;
        
        Debug.Assert(slot.CanSet(myData, myCount, InventorySlotSetMethod.Set) && CanSet(data, count, InventorySlotSetMethod.Set));

        slot.TrySet(myData, myCount, InventorySlotSetMethod.Set);
        TrySet(data, count, InventorySlotSetMethod.Set);
    }

    public void Clear()
    {
        Data = null;
        Count = 0;
                
        Chnaged?.Invoke(this);
    }

    public bool TrySet(ItemData itemData, int count, InventorySlotSetMethod method)
    {
        if (CanSet(itemData, count, method) == false) return false;
        
        if (itemData is null)
        {
            count = 0;
        }
        
        this.Data = itemData;

        switch (method)
        {
            case InventorySlotSetMethod.Set:
                this.Count = count;
                break;
            case InventorySlotSetMethod.Add:
                this.Count += count;
                break;
            default:
                break;
        }
        
        Chnaged?.Invoke(this);
        return true;
    }

}

public class Inventory : IInventory
{ 
    public IInventoryView View { get; private set; }
    protected readonly List<IInventorySlot> _slots = new();

    public IReadOnlyList<IInventorySlot> GetAllItem() => _slots;
    
    //TODO: 나중에 필요하면 작성
    public int MaxSize { get; }
    public IInventorySlot GetSlotSequentially(int index)
    {
        throw new NotImplementedException();
    }

    public void ViewRefresh()
    {
        
    }
}