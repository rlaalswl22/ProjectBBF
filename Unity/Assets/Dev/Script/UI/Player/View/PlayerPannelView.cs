using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPannelView : MonoBehaviour
{
    [SerializeField] private InteractableInventoryView _invView;
    [SerializeField] private SettingView _settingView;
    [SerializeField] private PlayerGameQuitView _quitView;

    public enum ViewType : int
    {
        Close,
        Inv,
        Setting,
        Quit,
    }

    private ViewType _viewState;
    public ViewType ViewState
    {
        get => _viewState;
        set
        {
            if (_viewState == ViewType.Close && value != ViewType.Close)
            {
                AudioManager.Instance.PlayOneShot("UI", "UI_Window_Open");
            }
            else if (_viewState != ViewType.Close && value == ViewType.Close)
            {
                AudioManager.Instance.PlayOneShot("UI", "UI_Window_Close");
            }
            
            _viewState = value;

            _settingView.Visible = false;
            _invView.Visible = false;
            _quitView.Visible = false;
            
            
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
                case ViewType.Quit:
                    _quitView.Visible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    // Unity Event Function
    public void SetViewState(int type)
    {
        ViewState = (ViewType)type;
        
        
        AudioManager.Instance.PlayOneShot("UI", "UI_Window_Click");
    }

    private void Awake()
    {
        _invView.Init();
        _settingView.Init();
        _quitView.Init();
        
        ViewState = ViewType.Close;
    }
}