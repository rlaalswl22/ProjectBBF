using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Actor/ActorMovementData", fileName = "NewActorMovementData")]
public class ActorMovementData : ScriptableObject
{
    [Serializable]
    public struct PathItem
    {
        public GameObject Path;
        public ESOGameTimeEvent ChangeTimeEvent;
    }

    [SerializeField] private float _movementSpeed;

    [SerializeField] private List<PathItem> _paths;


    public float MovementSpeed => _movementSpeed;
    public IReadOnlyList<PathItem> Paths => _paths;

}