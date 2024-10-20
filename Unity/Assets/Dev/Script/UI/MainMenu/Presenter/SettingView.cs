using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

public class SettingView : MonoBehaviour
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
    [SerializeField] private Slider _uiVolumeSlider;
    [SerializeField] private Slider _playerVolumeSlider;
    [SerializeField] private Slider _animalVolumeSlider;
    
    [SerializeField] private TMP_InputField _globalVolumeInputField;
    [SerializeField] private TMP_InputField _backgroundVolumeInputField;
    [SerializeField] private TMP_InputField _sfxVolumeInputField;
    [SerializeField] private TMP_InputField _uiVolumeInputField;
    [SerializeField] private TMP_InputField _playerVolumeInputField;
    [SerializeField] private TMP_InputField _animalVolumeInputField;
    
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
            
            AudioManager.Instance.PlayOneShot("UI", "UI_Window_Click");

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

    public void Add10Volume(string key)
    {
        if (key == "Global")
        {
            _globalVolumeSlider.value += 0.1f;
        }
        else if (key == "Background")
        {
            _backgroundVolumeSlider.value += 0.1f;
        }
        else if (key == "SFX")
        {
            _sfxVolumeSlider.value += 0.1f;
        }
        else if (key == "UI")
        {
            _sfxVolumeSlider.value += 0.1f;
        }
        else if (key == "Player")
        {
            _sfxVolumeSlider.value += 0.1f;
        }
        else if (key == "Animal")
        {
            _sfxVolumeSlider.value += 0.1f;
        }
    }
    public void Sub10Volume(string key)
    {
        if (key == "Global")
        {
            _globalVolumeSlider.value += -0.1f;
        }
        else if (key == "Background")
        {
            _backgroundVolumeSlider.value += -0.1f;
        }
        else if (key == "SFX")
        {
            _sfxVolumeSlider.value += -0.1f;
        }
        else if (key == "UI")
        {
            _sfxVolumeSlider.value += -0.1f;
        }
        else if (key == "Player")
        {
            _sfxVolumeSlider.value += -0.1f;
        }
        else if (key == "Animal")
        {
            _sfxVolumeSlider.value += -0.1f;
        }
    }

    private void OnSliderChanged(TMP_InputField inputField, string mixerGroupKey, float value)
    {
        int v = (int)(value * 100f);
        inputField.SetTextWithoutNotify($"{v.ToString()} / 100");

        var inst = AudioManager.Instance;
        if (inst)
        {
            inst.SetVolume(mixerGroupKey, value);
        }
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
        var resolutions = ScreenManager.Instance.AllResolutions;
        if (resolutions.Length <= i || i < 0) return;
        
        var resolution = resolutions[i];
        
        ScreenManager.Instance.SetResolution(resolution.width, resolution.height);
        
        _resolutionDropdown.SetValueWithoutNotify(i);
    }
    private void OnScreenMode(int i)
    {
        ScreenManager.Instance.ScreenMode = (FullScreenMode)(i + 1);
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
        _uiVolumeSlider.value = 1f;
        _playerVolumeSlider.value = 1f;
        _animalVolumeSlider.value = 1f;

        // sound
        _globalVolumeInputField.text = "100/100";
        _backgroundVolumeInputField.text =  "100/100";
        _sfxVolumeInputField.text = "100/100";
        _uiVolumeInputField.text = "100/100";
        _playerVolumeInputField.text = "100/100";
        _animalVolumeInputField.text = "100/100";
        
        _globalVolumeSlider.onValueChanged.AddListener(x=> OnSliderChanged(_globalVolumeInputField, "Global", x));
        _backgroundVolumeSlider.onValueChanged.AddListener(x=> OnSliderChanged(_backgroundVolumeInputField, "BGM", x));
        _sfxVolumeSlider.onValueChanged.AddListener(x=> OnSliderChanged(_sfxVolumeInputField, "SFX", x));
        _uiVolumeSlider.onValueChanged.AddListener(x=> OnSliderChanged(_uiVolumeInputField, "UI", x));
        _playerVolumeSlider.onValueChanged.AddListener(x=> OnSliderChanged(_playerVolumeInputField, "Player", x));
        _animalVolumeSlider.onValueChanged.AddListener(x=> OnSliderChanged(_animalVolumeInputField, "Animal", x));
        
        _globalVolumeInputField.onEndEdit.AddListener(x => OnInputFieldChanged(_globalVolumeSlider, _globalVolumeInputField, x));
        _backgroundVolumeInputField.onEndEdit.AddListener(x => OnInputFieldChanged(_backgroundVolumeSlider, _backgroundVolumeInputField, x));
        _sfxVolumeInputField.onEndEdit.AddListener(x => OnInputFieldChanged(_sfxVolumeSlider, _sfxVolumeInputField, x));
        _uiVolumeInputField.onEndEdit.AddListener(x => OnInputFieldChanged(_uiVolumeSlider, _uiVolumeInputField, x));
        _playerVolumeInputField.onEndEdit.AddListener(x => OnInputFieldChanged(_playerVolumeSlider, _playerVolumeInputField, x));
        _animalVolumeInputField.onEndEdit.AddListener(x => OnInputFieldChanged(_animalVolumeSlider, _animalVolumeInputField, x));

        
        
        // init..
        _screenModeDropdown.options = new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("전체 창 모드"),
            new TMP_Dropdown.OptionData("전체 화면"),
            new TMP_Dropdown.OptionData("창 모드"),
        };

        _resolutionDropdown.options = ScreenManager.Instance.AllResolutions
            .Select(x => new TMP_Dropdown.OptionData(x.ToString())).ToList();
        
        _vsyncDropdown.options = new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("활성화"),
            new TMP_Dropdown.OptionData("비활성화"),
        };

        
        
        _screenModeDropdown.onValueChanged.AddListener(OnScreenMode);
        _resolutionDropdown.onValueChanged.AddListener(OnResolution);
        _vsyncDropdown.onValueChanged.AddListener(OnVsync);

        _screenModeDropdown.value = (int)ScreenManager.Instance.ScreenMode - 1;
        _resolutionDropdown.value = ScreenManager.Instance.AllResolutions.Length - 1;
        _vsyncDropdown.value = QualitySettings.vSyncCount;
    }
}