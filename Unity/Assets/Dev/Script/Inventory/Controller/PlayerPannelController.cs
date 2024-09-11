using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPannelController : MonoBehaviour
{
    [SerializeField] private PlayerMainInventoryView _invView;
    [SerializeField] private PlayerSettingView _settingView;

    public enum ViewType : int
    {
        Close,
        Inv,
        Setting
    }

    private ViewType _viewState;
    public ViewType ViewState
    {
        get => _viewState;
        set
        {
            if (_viewState == value) return;
            _viewState = value;

            _settingView.Visible = false;
            _invView.Visible = false;
            
            
            gameObject.SetActive(_viewState != ViewType.Close);
            
            switch (_viewState)
            {
                case ViewType.Close:
                    break;
                case ViewType.Inv:
                    _invView.Visible = true;
                    break;
                case ViewType.Setting:
                    _settingView.Visible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void SetViewState(int type)
    {
        ViewState = (ViewType)type;
    }
    
    private void Start()
    {
        ViewState = ViewType.Close;
        gameObject.SetActive(false);
    }
}