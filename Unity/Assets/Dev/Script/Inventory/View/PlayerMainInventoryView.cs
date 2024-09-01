using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerMainInventoryView : MonoBehaviour, IInventoryView
{
    [SerializeField] private Transform _content;
    [SerializeField] private int _colCount = 4;

    private PlayerMainInventorySlotView[,] _slotViews;

    public int Row { get; private set; }
    public int Col { get; private set; }

    public bool Visible
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    private void Awake()
    {
        int length = _content.childCount;
        Row = length / _colCount + length % _colCount;
        Col = _colCount;
        
        _slotViews = new PlayerMainInventorySlotView
        [
            Row,
            Col
        ];

        int j = 0;
        for (int iter = 0; iter < length; iter++)
        {
            int i = iter / _colCount;

            if (j % _colCount == 0)
            {
                j = 0;
            }

            if (_content.GetChild(iter).TryGetComponent(out PlayerMainInventorySlotView slotView) is false)
            {
                Debug.LogError("컴포넌트가 존재하지 않음");
            }
            else
            {
                _slotViews[i, j] = slotView;
            }

            j++;
        }
    }

    public void Refresh(IInventoryModel model)
    {
       using var modelEnumerator = model.GetEnumerator();
        
        print(modelEnumerator);
        
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Col; j++)
            {
                if (modelEnumerator.MoveNext() is false) return;
                _slotViews[i, j].SlotController = modelEnumerator.Current;
            }
        }
    }
}
