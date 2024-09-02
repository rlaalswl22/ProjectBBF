﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMainInventorySlotView : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _slotImage;
    [SerializeField] private TMP_Text _text;
    private IInventorySlot _slotController;

    private void Awake()
    {
        _text.text = "";
        _slotImage.sprite = null;
    }

    public IInventorySlot SlotController
    {
        get => _slotController;
        set
        {
            if (_slotController == value) return;

            if (_slotController is not null)
            {
                _slotController.OnChanged -= OnChanged;
            }
            else
            {
                _text.text = "";
            }

            _slotController = value;
            _slotController.OnChanged += OnChanged;
        }
    }

    public ItemData ItemData => SlotController.Data;

    private void OnChanged(IInventorySlot slot)
    {
        _slotImage.sprite = slot.Data != null ? slot.Data.ItemSprite : null;

        if (slot.Data is not null && slot.Data.ActionCategoryType == ActionCategoryType.Tool)
        {
            _text.text = "";
            return;
        }

        _text.text = slot.Count == 0 ? "" : slot.Count.ToString();
    }

    private void OnClick(PointerEventData eventData)
    {
        Debug.Assert(_slotController is not null);
        

        var slot = SelectItemController.Instance.Model.Selected;
        if(slot.Data is null && _slotController.Data is null) return;

        bool halfFlag = eventData.button == PointerEventData.InputButton.Right;

        if (halfFlag && slot.Data == _slotController.Data)
        {
            PlaceOne(slot, _slotController);
        }
        else if (halfFlag && slot.Data is not null && _slotController.Data is null)
        {
            PlaceOne(slot, _slotController);
        }
        else if (halfFlag && slot.Data is null && _slotController.Data is not null)
        {
            PlaceHalf(slot, _slotController);
        }
        else if (halfFlag == false && slot.Data == _slotController.Data && _slotController.Count + slot.Count <= slot.Data.MaxStackCount)
        {
            Merge(slot, _slotController);
        }
        else
        {
            SwapItem(slot, _slotController);
        }
    }

    private void PlaceOne(IInventorySlot selected, IInventorySlot my)
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

    private void PlaceHalf(IInventorySlot selected, IInventorySlot my)
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

    private void Merge(IInventorySlot selected, IInventorySlot my)
    {
        SlotStatus status = my.TryAdd(selected.Count);
        if (SlotChecker.Contains(status, SlotStatus.Success))
        {
            selected.Clear();
        }
    }

    private void SwapItem(IInventorySlot selected, IInventorySlot my)
    {
        selected.Swap(my);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick(eventData);
    }
}