using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerQuickInventorySlotView : MonoBehaviour
{
    [SerializeField] private Image _slotImage;
    private IInventorySlot _slotController;

    public IInventorySlot SlotController
    {
        get => _slotController;
        set
        {
            if (_slotController == value) return;

            if (_slotController is not null)
            {
                _slotController.Chnaged -= OnChanged;
            }
            
            _slotController = value;
            _slotController.Chnaged += OnChanged;
        }
    }
    public ItemData ItemData => SlotController.Data;

    private void OnChanged(IInventorySlot slot)
    {
        _slotImage.sprite = slot.Data != null ? slot.Data.ItemSprite : null;
    }
}
