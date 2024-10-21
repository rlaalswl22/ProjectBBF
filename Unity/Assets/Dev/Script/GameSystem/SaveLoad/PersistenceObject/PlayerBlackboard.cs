using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ProjectBBF.Persistence;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;


[GameData]
[Serializable]
public class PlayerBlackboard : ISaveLoadNotification
{

    [Serializable]
    public struct GridModelSerialized
    {
        public List<Vector2Int> Pos;
        public List<string> ItemKey;
        public List<int> Count;

        public bool Empty
        {
            get
            {
                if (Pos is null || ItemKey is null || Count is null) return true;
                if(Pos.Count == 0 || ItemKey.Count == 0 || Count.Count == 0) return false;

                return true;
            }
        }
    }
    [Serializable]
    public struct A
    {
        public int a;
    }

    public A a;
    
    [NonSerialized, Editable] private float _stemina;
    [NonSerialized, Editable] private float _maxStemina;

    [Editable] private int _energy = 50;
    [Editable] private int _maxEnergy = 50;

    private string _currentWorld;
    private Vector2 _currentPosition;

    [SerializeField, Editable] private int _money = 500;

    private PlayerInventoryPresenter _inventory;

    public PlayerInventoryPresenter Inventory
    {
        get => _inventory;
        set => _inventory = value;
    }
    
    public bool IsMoveStopped { get; set; }
    public bool IsInteractionStopped { get; set; }
    public bool IsFishingStopped { get; set; }

    [SerializeField, Editable] private GridModelSerialized _serializedGridModel;


    public float Stemina
    {
        get => 999f;
        //get => _stemina;
        set => _stemina = Mathf.Clamp(value, 0f, MaxStemina);
    }

    public float MaxStemina
    {
        get => 999f;
        //get => _maxStemina;
        set => _maxStemina = value;
    }

    public int Energy
    {
        get => 999;
        //get => _energy;
        set => _energy = Mathf.Clamp(value, 0, MaxEnergy);
    }

    public int MaxEnergy
    {
        get => 999;
        //get => _maxEnergy;
        set => _maxEnergy = value;
    }

    public string CurrentWorld
    {
        get => _currentWorld;
        set => _currentWorld = value;
    }

    public Vector2 CurrentPosition
    {
        get => _currentPosition;
        set => _currentPosition = value;
    }

    public int Money
    {
        get => _money;
        set => _money = Mathf.Clamp(value, 0, int.MaxValue);
    }

    public GridInventoryModel CreateInventoryModelModel()
    {
        if (_serializedGridModel.Pos is null || _serializedGridModel.Pos?.Count == 0)
        {
            return new GridInventoryModel(new Vector2Int(10, 2));
        }

        var keys = _serializedGridModel.ItemKey.Distinct();

        List<(Vector2Int pos, ItemData item, int count)> items = new List<(Vector2Int pos, ItemData item, int count)>(_serializedGridModel.Pos.Count);


        AsyncOperationHandle<IList<ItemData>> itemHandle = Addressables.LoadAssetsAsync<ItemData>(keys, null, Addressables.MergeMode.Union);
        itemHandle.WaitForCompletion();

        Dictionary<string, ItemData> table = new Dictionary<string, ItemData>(itemHandle.Result.Select(x=>new KeyValuePair<string, ItemData>(x.ItemKey, x)));
        
        for (int i = 0; i < _serializedGridModel.ItemKey.Count; i++)
        {
            if (table.TryGetValue(_serializedGridModel.ItemKey[i], out var item) is false)
            {
                Debug.LogError($"불러오지 못한 아이템이 있습니다. ({_serializedGridModel.ItemKey[i]})");
                continue;
            }
            items.Add((_serializedGridModel.Pos[i], item, _serializedGridModel.Count[i]));
        }

        return new GridInventoryModel(items, new Vector2Int(10, 2));
    }

    public void OnSavedNotify()
    {
        if (Inventory?.Model is null) return;

        using var e = Inventory.Model.GetEnumerator();
        _serializedGridModel = new GridModelSerialized
        {
            ItemKey = new List<string>(Inventory.Model.MaxSize),
            Count = new List<int>(Inventory.Model.MaxSize),
            Pos = new List<Vector2Int>(Inventory.Model.MaxSize)
        };

        while (e.MoveNext())
        {
            if(e.Current is not GridInventorySlot slot) return;
            if (slot?.Empty ?? true) continue;
            if (slot.Data == false) continue;
            
            _serializedGridModel.ItemKey.Add(slot.Data.ItemKey);
            _serializedGridModel.Count.Add(slot.Count);
            _serializedGridModel.Pos.Add(slot.Position);
        }

    }

    public void OnLoadedNotify()
    {
        if (_serializedGridModel.Pos is null) return;
        
        int length = Mathf.Min(
            _serializedGridModel.Pos.Count,
            _serializedGridModel.ItemKey.Count,
            _serializedGridModel.Count.Count
            );
        
        Debug.Assert(_serializedGridModel.Pos.Count == length);
        Debug.Assert(_serializedGridModel.ItemKey.Count == length);
        Debug.Assert(_serializedGridModel.Count.Count == length);
        
        
        
    }
}