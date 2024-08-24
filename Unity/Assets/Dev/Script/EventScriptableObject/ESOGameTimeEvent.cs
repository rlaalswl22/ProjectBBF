using System;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Event/Time/GameTimeEvent", fileName = "GameTimeEvent", order = 0)]
public class ESOGameTimeEvent : EventScriptableObjectT<GameTime>
{
    [System.Serializable]
    public enum Operation
    {
        Equal,
        NotEqual,
        GreaterThenEqual,
        Greater,
        Less,
        LessThenEqual
    }

    [SerializeField] private Operation _operation;
    [SerializeField] private GameTime _targetGameTime;

    public Operation OperationType => _operation;
    public GameTime TargetGameTime => _targetGameTime;
}