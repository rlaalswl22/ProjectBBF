


using System;
using MyBox;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashBoxUI : MonoBehaviour
{
    [field: SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    public void OnClick()
    {
        var item = SelectItemPresenter.Instance.Model.Selected;
        if (item is null) return;

        SelectItemPresenter.Instance.Model.Selected.Clear();
    }
}