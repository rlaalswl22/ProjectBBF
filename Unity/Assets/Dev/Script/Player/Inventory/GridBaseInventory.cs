using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridInventorySlot : InventorySlot
{
    public Vector2Int Position { get; private set; }

    public GridInventorySlot(Vector2Int position)
    {
        this.Position = Position;
    }
}

public class GridInventory : IInventory
{
    public GridInventorySlot[,] Slots { get; private set; }
    public Vector2Int Size { get; private set; }
    
    public bool IsFull => GetFirstEmptySlotPosition() is null;

    public GridInventory(Vector2Int defaultSize)
    {
        Alloc(defaultSize);
    }

    private void Alloc(Vector2Int newSize)
    {
        var newSlot = new GridInventorySlot[newSize.y, newSize.x];
        Size = newSize;
        
        for (int i = 0; i < Size.y; i++)
        {
            for (int j = 0; j < Size.x; j++)
            {
                GridInventorySlot gridSlot= new GridInventorySlot(new Vector2Int(j, i));
                newSlot[i, j] = gridSlot;
            }
        }

        Slots = newSlot;
    }
    
    /// <summary>
    /// 인벤토리의 크기를 재할당함.
    /// </summary>
    /// <param name="newSize">재할당할 인벤토리의 크기</param>
    /// <returns>만약, 매개변수의 크기가, 현재 인벤토리 크기보다 작다면 false를 반환함.</returns>
    public bool Realloc(Vector2Int newSize)
    {
        if (newSize.sqrMagnitude < Size.sqrMagnitude)
        {
            return false;
        }
        
        var originSlot = Slots;
        var newSlot = new GridInventorySlot[newSize.x, newSize.y];

        var eumerator = Slots.GetEnumerator();
        
        for (int i = 0; i < newSize.y; i++)
        {
            for (int j = 0; j < newSize.x; j++)
            {
                GridInventorySlot gridSlot= new GridInventorySlot(new Vector2Int(j, i));
                newSlot[i, j] = gridSlot;
                
                if (eumerator.MoveNext() && eumerator.Current is GridInventorySlot originGridSlot)
                {
                    gridSlot.Swap(originGridSlot);
                }
            }
        }

        Slots = newSlot;
        Size = newSize;
        
        return true;
    }
    
    /// <summary>
    /// 인벤토리에서 가장 첫번째로 비어있는 슬롯의 포지션을 반환.
    /// </summary>
    /// <returns>인벤토리가 꽉 차있다면 null을 반환.</returns>
    public Vector2Int? GetFirstEmptySlotPosition()
    {
        for (int i = 0; i < Size.y; i++)
        {
            for (int j = 0; j < Size.x; j++)
            {
                if (Slots[i, j].Empty)
                {
                    return new Vector2Int(j, i);
                }
            }
        }

        return null;
    }


    public Vector2Int? GetAdditionalSlotPosition(ItemData itemData, int count)
    {
        for (int i = 0; i < Size.y; i++)
        {
            for (int j = 0; j < Size.x; j++)
            {
                var slot = Slots[i, j];
                if (slot.Data == itemData && itemData.MaxStackCount >= slot.Count + count)
                {
                    return new Vector2Int(j, i);
                }
            }
        }

        return null;
    }

    public bool PushItem(ItemData itemData, int count)
    {
        Vector2Int? slotPos = GetAdditionalSlotPosition(itemData, count);
        InventorySlotSetMethod method;

        if (slotPos is not null)
        {
            method = InventorySlotSetMethod.Add;
        }
        else
        {
            slotPos = GetFirstEmptySlotPosition();
            
            if (slotPos is null) return false;
            method = InventorySlotSetMethod.Set;
        }
        

        var slot = Slots[slotPos!.Value.y, slotPos!.Value.x];
        return slot.TrySet(itemData, count, method);
    }

    public int MaxSize => Size.sqrMagnitude;

    public IInventorySlot GetSlotSequentially(int index)
    {
        var iter = Slots.GetEnumerator();
        for (int i = 0; i <= index; i++)
        {
            if (iter.MoveNext() == false) return null;
        }

        return iter.Current as IInventorySlot;
    }

    public void ViewRefresh()
    {
        
    }
}