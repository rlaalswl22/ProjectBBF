using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearInventoryModel : IInventoryModel
{
    public int MaxSize => -1;
    public int CurrentSize { get; private set; }
    
    private List<DefaultInventorySlot> _slots = new List<DefaultInventorySlot>();

    public LinearInventoryModel()
    {
    }

    public IInventorySlot GetSlotSequentially(int index)
    {
        if (index >= _slots.Count || index < 0) return null;
        
        return _slots[index];
    }

    public IEnumerator<IInventorySlot> GetEnumerator()
        => _slots.GetEnumerator();

    public void PushItem(ItemData item, int count)
    {
        var slot = FirstPushableSlot(item, count);

        if (slot is null)
        {
            slot = new DefaultInventorySlot(true);
        }
        
        bool success = slot.TryAdd(count);
        Debug.Assert(success);
            
        _slots.Add(slot);
    }
    
    public DefaultInventorySlot FirstPushableSlot(ItemData item, int count)
    {
        var iter = _slots.GetEnumerator();

        while (iter.MoveNext())
        {
            if (iter.Current is not null && iter.Current.CanAdd(count))
            {
                return iter.Current;
            }
        }

        return null;
    }

    public bool Contains(ItemData item)
    {
        var iter = _slots.GetEnumerator();

        while (iter.MoveNext())
        {
            if (iter.Current is not null && iter.Current.Data == item)
            {
                return true;
            }
        }

        return false;
    }
}