using System;
using ProjectBBF.Persistence;
using UnityEngine;

[Serializable]
public class PlayerBlackboard : IPersistenceObject
{
    [NonSerialized] private float _stemina;
    [NonSerialized] private float _maxStemina;

    [SerializeField] private int _energy = 50;
    [SerializeField] private int _maxEnergy = 50;

    public float Stemina
    {
        get => _stemina;
        set => _stemina = Mathf.Clamp(value, 0f, MaxStemina);
    }

    public float MaxStemina
    {
        get => _maxStemina;
        set => _maxStemina = value;
    }

    public int Energy
    {
        get => _energy;
        set => _energy = Mathf.Clamp(value, 0, MaxEnergy);
    }

    public int MaxEnergy
    {
        get => _maxEnergy;
        set => _maxEnergy = value;
    }
}