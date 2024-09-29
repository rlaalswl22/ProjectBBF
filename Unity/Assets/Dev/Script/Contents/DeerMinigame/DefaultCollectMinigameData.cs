using System.Collections;
using System.Collections.Generic;
using DS.Core;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Minigame/Default Collect", fileName = "New default collect")]
public class DefaultCollectMinigameData : CollectMinigameDataBase
{

    [SerializeField, Header("보상")] private List<ItemDataSerializedSet> _rewards;
    
    [SerializeField, Header("튜토리얼 대사")] private DialogueContainer _tutorial;

    public DialogueContainer Tutorial => _tutorial;
    public IReadOnlyList<ItemDataSerializedSet> Rewards => _rewards;

}
