using System;
using ProjectBBF.Persistence;
using UnityEngine;

public class ContestResultTest : MonoBehaviour
{
    [SerializeField] private ESOContestResultPush _eso;
    [SerializeField] private ItemData _itemData;
    [SerializeField] private SceneName _testGoBackScene;
    
    private void Start()
    {
        var board=  PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");
        
        board.CurrentWorld = _testGoBackScene.Scene;
        
        _eso.Raise(new ContestResultEvent()
        {
            TargetItem = _itemData
        });
    }
}