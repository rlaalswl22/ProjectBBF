using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum GameScreenResolution : int
{
    _960_540,
    _1920_1080,
    _2560_1440,
    _3840_2160
}

public enum GameScreenMode : int
{
    FullScreen,
    FullScreenWindow,
    Windowed
}
public enum GameVsync : int
{
    On,
    Off
}

public class PlayerSettingView : MonoBehaviour
{
    public enum VisibleState
    {
        Graphic,
        Volume
    }
    
    // sound
    [SerializeField] private Slider _globalVolumeSlider;
    [SerializeField] private Slider _backgroundVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    
    [SerializeField] private TMP_InputField _globalVolumeInputField;
    [SerializeField] private TMP_InputField _backgroundVolumeInputField;
    [SerializeField] private TMP_InputField _sfxVolumeInputField;
    
    [SerializeField] private GameObject _soundFrame;
    
    // graphic
    [SerializeField] private TMP_Dropdown _screenModeDropdown;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _vsyncDropdown;
    
    [SerializeField] private GameObject _graphicFrame;
    public bool Visible
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    private VisibleState _state;
    public VisibleState State
    {
        get => _state;
        set
        {
            _state = value;
            
            _soundFrame.SetActive(false);
            _graphicFrame.SetActive(false);

            if (_state == VisibleState.Graphic)
            {
                _graphicFrame.SetActive(true);
            }
            else if (_state == VisibleState.Volume)
            {
                _soundFrame.SetActive(true);
            }
        }
    }

    public void SetState(int state)
    {
        State = (VisibleState)state;
    }

    private void OnSliderChanged(TMP_InputField inputField, float value)
    {
        int v = (int)(value * 100f);
        inputField.SetTextWithoutNotify($"{v.ToString()} / 100");
    }
    private void OnInputFieldChanged(Slider slider, TMP_InputField inputField, string value)
    {
        if (int.TryParse(value, out int v) is false) return;

        if (v > 100)
        {
            v = 100;
        }

        if (v < 0)
        {
            v = 0;
        }
            
        slider.value = v / 100f;
        inputField.SetTextWithoutNotify($"{v.ToString()} / 100");
    }

    private void OnResolution(int i)
    {
        if (i == (int)GameScreenResolution._960_540)
        {
            Screen.SetResolution(960, 540, Screen.fullScreenMode);
        }
        if (i == (int)GameScreenResolution._1920_1080)
        {
            Screen.SetResolution(2560, 1440, Screen.fullScreenMode);
        }
        if (i == (int)GameScreenResolution._2560_1440)
        {
            Screen.SetResolution(2560, 1440, Screen.fullScreenMode);
        }
        if (i == (int)GameScreenResolution._3840_2160)
        {
            Screen.SetResolution(3840, 2160, Screen.fullScreenMode);
        }
        
        
        _resolutionDropdown.SetValueWithoutNotify(i);
    }
    private void OnScreenMode(int i)
    {
        if (i == (int)GameScreenMode.FullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
        }
        if (i == (int)GameScreenMode.FullScreenWindow)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        if (i == (int)GameScreenMode.Windowed)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        
        _screenModeDropdown.SetValueWithoutNotify(i);
    }
    private void OnVsync(int i)
    {
        QualitySettings.vSyncCount = (GameVsync)i == GameVsync.On ? 1 : 0;
        _vsyncDropdown.SetValueWithoutNotify(i);
    }

    public void Init()
    {
        // graphic
        _globalVolumeSlider.value = 1f;
        _backgroundVolumeSlider.value = 1f;
        _sfxVolumeSlider.value = 1f;

        _globalVolumeInputField.text = "100/100";
        _backgroundVolumeInputField.text =  "100/100";
        _sfxVolumeInputField.text = "100/100";
        
        _globalVolumeSlider.onValueChanged.AddListener(x=> OnSliderChanged(_globalVolumeInputField, x));
        _backgroundVolumeSlider.onValueChanged.AddListener(x=> OnSliderChanged(_backgroundVolumeInputField, x));
        _sfxVolumeSlider.onValueChanged.AddListener(x=> OnSliderChanged(_sfxVolumeInputField, x));
        
        _globalVolumeInputField.onEndEdit.AddListener(x => OnInputFieldChanged(_globalVolumeSlider, _globalVolumeInputField, x));
        _backgroundVolumeInputField.onEndEdit.AddListener(x => OnInputFieldChanged(_backgroundVolumeSlider, _backgroundVolumeInputField, x));
        _sfxVolumeInputField.onEndEdit.AddListener(x => OnInputFieldChanged(_sfxVolumeSlider, _sfxVolumeInputField, x));

        // sound
        _screenModeDropdown.options = new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("전체 화면"),
            new TMP_Dropdown.OptionData("전체 창 모드"),
            new TMP_Dropdown.OptionData("창 모드"),
        };
        OnScreenMode((int)GameScreenMode.FullScreen);
        
        _resolutionDropdown.options = new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("960 x 540"),
            new TMP_Dropdown.OptionData("1920 x 1080"),
            new TMP_Dropdown.OptionData("2560 x 1440"),
            new TMP_Dropdown.OptionData("3840 x 2160"),
        };
        
        _vsyncDropdown.options = new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("활성화"),
            new TMP_Dropdown.OptionData("비활성화"),
        };
        OnVsync((int)GameScreenMode.FullScreen);

        if (Screen.fullScreenMode == FullScreenMode.MaximizedWindow)
        {
            _screenModeDropdown.value = (int)GameScreenMode.FullScreenWindow;
        }
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            _screenModeDropdown.value = (int)GameScreenMode.FullScreenWindow;
        }
        else if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            _screenModeDropdown.value = (int)GameScreenMode.Windowed;
        }
        

        if (Screen.fullScreenMode == FullScreenMode.MaximizedWindow)
        {
            _screenModeDropdown.value = (int)GameScreenMode.FullScreenWindow;
        }
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            _screenModeDropdown.value = (int)GameScreenMode.FullScreenWindow;
        }
        else if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            _screenModeDropdown.value = (int)GameScreenMode.Windowed;
        }

        if (Screen.currentResolution.width == 960 && Screen.currentResolution.height == 540)
        {
            _resolutionDropdown.value = (int)GameScreenResolution._960_540;
        }
        if (Screen.currentResolution.width == 1920 && Screen.currentResolution.height == 1080)
        {
            _resolutionDropdown.value = (int)GameScreenResolution._1920_1080;
        }
        if (Screen.currentResolution.width == 2560 && Screen.currentResolution.height == 1440)
        {
            _resolutionDropdown.value = (int)GameScreenResolution._2560_1440;
        }
        if (Screen.currentResolution.width == 3840 && Screen.currentResolution.height == 2160)
        {
            _resolutionDropdown.value = (int)GameScreenResolution._3840_2160;
        }
        
        _vsyncDropdown.value = QualitySettings.vSyncCount;
        
        
        _screenModeDropdown.onValueChanged.AddListener(OnScreenMode);
        _resolutionDropdown.onValueChanged.AddListener(OnResolution);
        _vsyncDropdown.onValueChanged.AddListener(OnVsync);
    }
}