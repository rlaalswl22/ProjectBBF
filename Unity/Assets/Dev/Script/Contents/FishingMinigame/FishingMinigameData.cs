using System;
using System.Collections;
using System.Collections.Generic;
using DS.Core;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Minigame/Fishing", fileName = "FishingMinigameData")]
public class FishingMinigameData : MinigameData
{
    [SerializeField, Header("튜토리얼 대사")] private DialogueContainer _tutorial;

    [SerializeField, Header("물고기가 찌를 무는 간격(초)")] private float _biteRepeatInterval;
    [SerializeField, Header("물고기가 찌를 물고있는 시간")] private float _biteCanBiteDuration;
    [SerializeField, Header("게임 제한시간")] private float _gameDuration;

    [SerializeField, Header("보상")] private List<FishingReward> _rewards;

    public DialogueContainer Tutorial => _tutorial;

    public float BiteRepeatInterval => _biteRepeatInterval;

    public float BiteCanBiteDuration => _biteCanBiteDuration;

    public List<FishingReward> Rewards => _rewards;

    public float GameDuration => _gameDuration;
}

[Serializable]
public struct FishingReward
{
    public ItemData Item;
    
    [Range(0f, 1f)]
    public float Percentage;
    
}