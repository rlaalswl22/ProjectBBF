using System;
using System.Collections;
using System.Collections.Generic;
using DS.Core;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Minigame/Fishing", fileName = "FishingMinigameData")]
public class FishingMinigameData : MinigameData
{
    [SerializeField] private DialogueContainer _tutorial;

    [SerializeField] private float _biteRepeatInterval;
    [SerializeField] private float _biteCanBiteDuration;
    [SerializeField] private float _gameDuration;

    [SerializeField] private List<FishingReward> _rewards;

    public DialogueContainer Tutorial => _tutorial;

    public float BiteRepeatInterval => _biteRepeatInterval;

    public float BiteCanBiteDuration => _biteCanBiteDuration;

    public new List<FishingReward> Rewards => _rewards;

    public float GameDuration => _gameDuration;
}

[Serializable]
public struct FishingReward
{
    public ItemData Item;
    
    [Range(0f, 1f)]
    public float Percentage;
    
}