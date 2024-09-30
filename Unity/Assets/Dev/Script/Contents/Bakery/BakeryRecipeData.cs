using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Bakery/Bakery Recipe Data", fileName = "BakeryRecipeData")]
public class BakeryRecipeData : ScriptableObject
{
    [SerializeField] private string _key;
    
    [SerializeField] private ItemData _resultItem;
    
    [SerializeField] private ItemData _doughtItem0;
    [SerializeField] private ItemData _doughtItem1;
    [SerializeField] private ItemData _doughtItem2;
    
    [SerializeField] private ItemData _additiveItem0;
    
    private ItemData[] _doughtItems;
    private ItemData[] _additiveItems;

    public ItemData ResultItem => _resultItem;

    public ItemData[] DoughtItems
    {
        get
        {
            if (_doughtItems is null)
            {
                _doughtItems = new ItemData[]
                {
                    _doughtItem0,
                    _doughtItem1,
                    _doughtItem2,
                };
            }
            
            return _doughtItems;
        }
    }


    public ItemData[] AdditiveItem
    {
        get
        {
            if (_additiveItems is null)
            {
                _additiveItems = new ItemData[]
                {
                    _additiveItem0
                };
            }
            
            return _additiveItems;
        }
    }

    public string Key => _key;

    private void OnValidate()
    {
        _doughtItems = null;
        _additiveItems = null;
    }
}
