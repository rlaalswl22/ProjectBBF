


using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RecipeBookMarkerView : MonoBehaviour
{
    [SerializeField] private Sprite _enabledSprite;
    [SerializeField] private Sprite _disabledSprite;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        Debug.Assert(_button);
    }

    public Button Button => _button;

    public bool IsBookmarked
    {
        set
        {
            _button.image.sprite = value ? _enabledSprite : _disabledSprite;
        }
    }
}