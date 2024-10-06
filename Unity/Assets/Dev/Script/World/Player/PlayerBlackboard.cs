using System;
using System.Runtime.Serialization;
using ProjectBBF.Persistence;
using UnityEngine;

[GameData]
[Serializable]
public class PlayerBlackboard
{
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
        set => _money = value;
    }
}