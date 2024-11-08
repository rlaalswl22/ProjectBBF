


using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RecipeBookMarkerView : MonoBehaviour
{
    [SerializeField] private Sprite _enabledSprite;
    [SerializeField] private Sprite _disabledSprite;

    private Button _button;
    public Button Button
    {
        get
        {
            if(_button == false)
            {
                _button = GetComponent<Button>();
                Debug.Assert(_button);
            }

            return _button;
        }
    }

    public bool IsBookmarked
    {
        set => Button.image.sprite = value ? _enabledSprite : _disabledSprite;
    }
}