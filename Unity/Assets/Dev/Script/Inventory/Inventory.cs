using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventorySlot
{
    public event Action<IInventorySlot> OnChanged;
    public ItemData Data { get; }
    public int Count { get; }
    public bool Empty { get; }

    public bool TrySet(ItemData itemData, int count);
    public bool TryAdd(int count, bool allowZeroEmpty = false);

    public bool CanAdd(int count, bool allowZero = false);
    public bool CanSet(ItemData itemData, int count);
    public void Swap(IInventorySlot slot);
    public void Clear();
}

public interface IInventoryModel
{
    /// <summary>
    /// 인벤토리의 최대 크기.
    /// 값이 -1 인 경우, 최대 크기가 없음을 나타냄.
    /// </summary>
    public int MaxSize { get; }
    /// <summary>
    /// 인벤토리의 index 번째의 슬롯을 반환.
    /// 모든 구현체들은 선형적인 index를 가공하여 논리적으로 올바른 슬롯을 반환해야함. 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IInventorySlot GetSlotSequentially(int index);
    /// <summary>
    /// 모든 인벤토리는 IEnumerator를 통해 순회가 가능하도록 해야함.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<IInventorySlot> GetEnumerator();
    
    public bool Contains(ItemData itemData);
}
public interface IInventoryController<out TModel>
    where TModel : IInventoryModel
{
    public TModel Model { get; }
}

public interface IInventoryView
{
    public void Refresh(IInventoryModel model);
}

public enum InventorySlotSetMethod
{
    Set,
    Add
}

public class DefaultInventorySlot : IInventorySlot
{
    public event Action<IInventorySlot> OnChanged;
    public ItemData Data { get; private set; }
    public int Count { get; private set; }

    public bool Empty => Data is null;
    
    public bool InfinityCount { get; set; }

    public DefaultInventorySlot(bool infiniteCount = false)
    {
        InfinityCount = infiniteCount;
    }

    public bool CanSet(ItemData itemData, int count)
    {
        if (itemData is null)
        {
            return true;
        }

        if (count < 1 || (count > itemData.MaxStackCount & InfinityCount is false))
        {
            return false;
        }

        return true;
    }

    public bool CanAdd(int count, bool allowZero= false)
    {
        if (Data is null)
        {
            return true;
        }
        
        // < 1 (true) & false = 0
        // < 1 (true) & true = 1
        // < 1 (false) & false = 0
        // < 1 (false) & true = 0
        if ((count + this.Count > Data.MaxStackCount & InfinityCount is false) || (count + this.Count < 1 & allowZero is false))
        {
            return false;
        }

        return true;
    }

    public void Swap(IInventorySlot slot)
    {
        var data = slot.Data;
        var count = slot.Count;

        var myData = Data;
        var myCount = Count;

        Debug.Assert(slot.CanSet(myData, myCount) &&
                     CanSet(data, count));

        slot.TrySet(myData, myCount);
        TrySet(data, count);
    }

    public void Clear()
    {
        Data = null;
        Count = 0;

        OnChanged?.Invoke(this);
    }

    public bool TrySet(ItemData itemData, int count)
    {
        if (CanSet(itemData, count) == false) return false;

        this.Data = itemData;
        this.Count = count;

        OnChanged?.Invoke(this);
        return true;
    }

    public bool TryAdd(int count, bool allowZeroEmpty = false)
    {
        if (CanAdd(count, allowZeroEmpty) == false)
        {
            return false;
        }

        if (allowZeroEmpty && this.Count + count < 1)
        {
            this.Data = null;
        }

        this.Count = Mathf.Clamp(this.Count + count, 0, Data?.MaxStackCount ?? 0);
        
        OnChanged?.Invoke(this);
        return true;
    }
}