using System;
using DS.Core;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Minigame/FrogRace", fileName = "FrogRace")]
public class FrogRaceMinigameData : MinigameData
{
    [SerializeField] private DialogueContainer _tutorial;

    [SerializeField] private float _boostMovementMultiplier;
    [SerializeField] private float _boostMaxIteration;
    [SerializeField] private float _jumpInterval;
    [SerializeField] private float _jumpMinDistance;
    [SerializeField] private float _jumpMaxDistance;

    [SerializeField] private float _movementSpeed;


    [SerializeField] private float _jumpMaxHeight;

    [Serializable]
    public struct FrogData
    {
        public FrogRaceFrogObject FrogObject;
        public string DisplayName;
        public float DividendRate;
        public float JumpRate;
        public float JumpBoostRate;
    }

    public DialogueContainer Tutorial => _tutorial;

    [SerializeField] private FrogData[] _frogs;

    public float BoostMovementMultiplier => _boostMovementMultiplier;

    public float BoostMaxIteration => _boostMaxIteration;

    public float JumpInterval => _jumpInterval;

    public float JumpMinDistance => _jumpMinDistance;

    public float JumpMaxDistance => _jumpMaxDistance;


    public float MovementSpeed => _movementSpeed;

    public float JumpMaxHeight => _jumpMaxHeight;

    public FrogData[] Frogs => _frogs;
}