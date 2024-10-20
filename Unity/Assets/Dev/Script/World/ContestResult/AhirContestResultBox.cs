using System.Collections;
using System.Collections.Generic;
using DS.Runtime;
using ProjectBBF.Event;
using ProjectBBF.Persistence;
using UnityEngine;

public class AhirContestResultBox : BakeryFlowBehaviourBucket
{
    [SerializeField] private string doOnceKey;

    public static ItemData ResultItem { get; private set; }
    
    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        ResultItem = null;
    }
    
    protected override bool CanStore(int index, ItemData itemData)
    {
        if (index != 0) return false;

        var inst = BakeryRecipeResolver.Instance;
        
        return inst.CanListOnBakedBread(itemData) || 
               inst.CanListOnCompletionBread(itemData) ||
               inst.FailBakedBreadRecipe.BreadItem == itemData ||
               inst.FailResultBreadRecipe.ResultItem == itemData;
    }

    protected override void OnChangedBuket(int index, ItemData itemData)
    {
        base.OnChangedBuket(index, itemData);
        ResultItem = itemData;
        
        var blackboard = PersistenceManager.Instance.LoadOrCreate<DoOnceHandlerPersistenceObject>("DoOnce");
        if (itemData)
        {
            if (blackboard.DoOnceList.Contains(doOnceKey) is false)
            {
                blackboard.DoOnceList.Add(doOnceKey);
            }
        }
        else
        {
            blackboard.DoOnceList.Remove(doOnceKey);
        }
    }

    protected override void OnActivate(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
        
    }

    protected override void OnEnter(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
    }

    protected override void OnExit(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
    }
}
