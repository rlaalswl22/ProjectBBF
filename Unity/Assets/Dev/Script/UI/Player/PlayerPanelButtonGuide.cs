using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerPanelButtonGuide : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private ItemToolTipView _toolTipView;
    [SerializeField] private string _toolTipText;

    private void Awake()
    {
        if (_toolTipView)
        {
            _toolTipView.Clear();
            _toolTipView.Visible = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_toolTipView)
        {
            if (SelectItemPresenter.Instance.Model.IsEmpty is false) return;

            _toolTipView.Visible = true;
            _toolTipView.ItemNameDisplayText = _toolTipText;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_toolTipView)
        {
            _toolTipView.Visible = false;
            _toolTipView.Clear();
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (_toolTipView)
        {
            var pos = ItemToolTipView.ScreenToOrthogonal(eventData.position);
            pos = _toolTipView.ToValidPosition(pos);
            pos = ItemToolTipView.OrthogonalToScreen(pos);

            _toolTipView.SetPositionWithOffset(pos);
        }
    }
}
