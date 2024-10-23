using System.Collections;
using System.Collections.Generic;
using DS.Core;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Minigame/Farm", fileName = "FarmMinigameData")]
public class FarmMinigameData : MinigameData
{
    [SerializeField, Header("농작물 성장 간격(초)")]
    private float _grownInterval;

    [SerializeField, Header("목표 아이템 개수")] private int _goalItemCount;
    [SerializeField, Header("목표 아이템")] private ItemData _goalItem;

    [SerializeField, Header("튜토리얼 대사")] private DialogueContainer _dialogueTutorial;

    public float GrownInterval => _grownInterval;

    public int GoalItemCount => _goalItemCount;

    public ItemData GoalItem => _goalItem;

    public DialogueContainer DialogueTutorial => _dialogueTutorial;
}
