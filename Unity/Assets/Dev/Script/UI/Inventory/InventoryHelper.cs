using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public static class InventoryHelper
{
    public static void QuickMoveWithGridModel(IInventorySlot slot, GridInventoryModel target)
    {
        Debug.Assert(slot is not null);
        Debug.Assert(target is not null);
        
        if ( slot.Data)
        {
            int remainCount = target.PushItem(slot.Data, slot.Count);

            if (remainCount != 0)
            {
                slot.ForceSet(slot.Data, remainCount);
            }
            else
            {
                slot.Clear();
            }
        }
    }
    public static void SwapOrHalfItem(IInventorySlot invSlot, PointerEventData eventData)
    {
        Debug.Assert(invSlot is not null);
        

        var slot = SelectItemPresenter.Instance.Model.Selected;
        if(slot.Data is null && invSlot.Data is null) return;

        bool halfFlag = eventData.button == PointerEventData.InputButton.Right;

        if (halfFlag && slot.Data == invSlot.Data)
        {
            PlaceOne(slot, invSlot);
        }
        else if (halfFlag && slot.Data is not null && invSlot.Data is null)
        {
            PlaceOne(slot, invSlot);
        }
        else if (halfFlag && slot.Data is null && invSlot.Data is not null)
        {
            PlaceHalf(slot, invSlot);
        }
        else if (halfFlag == false && slot.Data == invSlot.Data && slot.Data && invSlot.Count + slot.Count <= slot.Data.MaxStackCount)
        {
            Merge(slot, invSlot);
        }
        else
        {
            SwapItem(slot, invSlot);
        }
        
        
        //AudioManager.Instance.PlayOneShot("UI", "UI_MouseOver");
    }

    private static void PlaceOne(IInventorySlot selected, IInventorySlot my)
    {
        if (my.Data && my.Count >= my.Data.MaxStackCount)
        {
            SwapItem(selected, my);
            return;
        }
        
        var data = selected.Data;
        SlotStatus status = selected.TryAdd(-1, true);
        if (SlotChecker.Contains(status, SlotStatus.Success))
        {
            status = my.TryAdd(1);
            if (SlotChecker.Contains(status, SlotStatus.NullData))
            {
                my.TrySet(data, 1);
            }
        }
    }

    private static void PlaceHalf(IInventorySlot selected, IInventorySlot my)
    {
        if (my.Count == 1)
        {
            selected.Clear();
            selected.TrySet(my.Data, 1);
            my.Clear();
            return;
        }

        int myCount = my.Count;
        SlotStatus status = my.TrySet(my.Data, my.Count / 2);
        if (SlotChecker.Contains(status, SlotStatus.Success))
        {
            selected.TrySet(my.Data, myCount / 2 + myCount % 2);
        }
    }

    private static void Merge(IInventorySlot selected, IInventorySlot my)
    {
        SlotStatus status = my.TryAdd(selected.Count);
        if (SlotChecker.Contains(status, SlotStatus.Success))
        {
            selected.Clear();
        }
    }

    private static void SwapItem(IInventorySlot selected, IInventorySlot my)
    {
        selected.Swap(my);
    }
}