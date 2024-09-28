using System;
using System.Collections.Generic;
using DS.Core;
using MyBox;
using UnityEngine;


[CreateAssetMenu(menuName = "ProjectBBF/Data/Minigame/Mole", fileName = "MoleData")]
public class MoleMinigameData : MinigameData
{
    [Serializable]
    public struct Mole
    {
        public int Key;
        public MoleGameObject Prefab;
        public int AcquisitionScore;
        public float AppearRate;
        public float WaitDuration;
    }

    [Serializable]
    public struct Reward
    {
        public ItemData Item;
        public int Count;
        public int TargetScore;
    }

    [Serializable]
    public struct Stage
    {
        public float AppearInterval;
        public int AppearMaxCount;
        public List<int> MoleKeyList;
        public float MaxStageTime;
    }

    [SerializeField] private DialogueContainer _tutorial;
    
    [SerializeField] private ToolRequireSet _requireTools;

    [SerializeField] private float _gameDuration;
    [SerializeField] private List<Mole> _moles;

    [SerializeField] private List<Reward> _rewards;
    [SerializeField] private List<Stage> _stages;

    public ToolRequireSet RequireTools => _requireTools;

    public float GameDuration => _gameDuration;

    public List<Mole> Moles => _moles;
    public List<Reward> Rewards => _rewards;
    public List<Stage> Stages => _stages;

    public DialogueContainer Tutorial => _tutorial;
}