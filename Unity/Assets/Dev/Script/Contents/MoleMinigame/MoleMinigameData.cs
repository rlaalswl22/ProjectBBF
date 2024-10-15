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
         [Header("키 값(반드시 고유해야함")] public int Key;
         public MoleGameObject Prefab;
         [Header("피격시 획득 점수")] public int AcquisitionScore;
         [Header("나타날 확률")] public float AppearRate;
         [Header("구멍에 들어가기전 대기하는 시간")] public float WaitDuration;
    }

    [Serializable]
    public struct Reward
    {
        [Header("보상 아이템")]  public ItemData Item;
        [Header("보상 아이템 개수")]  public int Count;
        [Header("필요 점수")]  public int TargetScore;
        [Header("등수")] public int Rank;
    }

    [Serializable]
    public struct Stage
    {
        [Header("두더지 출현 빈도(초)")]  public float AppearInterval;
        [Header("두더지 출현 최대 개수")]  public int AppearMaxCount;
        [Header("출현 두더지 키(두더지의 출현 확률의 합이 1이어야 함")]  public List<int> MoleKeyList;
        [Header("스테이지 최대 시간")]  public float MaxStageTime;
    }

    [SerializeField, Header("튜토리얼 대사")] private DialogueContainer _tutorial;
    
    [SerializeField, Header("두더지 잡는 사용할 도구")] private ToolRequireSet _requireTools;

    [SerializeField, Header("게임 제한시간")] private float _gameDuration;
    [SerializeField, Header("두더지 정보")] private List<Mole> _moles;

    [SerializeField, Header("보상 정보")] private List<Reward> _rewards;
    [SerializeField, Header("스테이지 정보")] private List<Stage> _stages;

    public ToolRequireSet RequireTools => _requireTools;

    public float GameDuration => _gameDuration;

    public List<Mole> Moles => _moles;
    public List<Reward> Rewards => _rewards;
    public List<Stage> Stages => _stages;

    public DialogueContainer Tutorial => _tutorial;
}