using System.Collections;
using System.Collections.Generic;
using DS.Core;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Minigame/Default Collect", fileName = "New default collect")]
public class DefaultCollectMinigameData : CollectMinigameDataBase
{

    [SerializeField] private List<ItemDataSerializedSet> _rewards;
    
    [SerializeField] private DialogueContainer _tutorial;

    public DialogueContainer Tutorial => _tutorial;
    public IReadOnlyList<ItemDataSerializedSet> Rewards => _rewards;

}
