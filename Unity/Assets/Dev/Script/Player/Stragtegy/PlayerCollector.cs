using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerCollector : MonoBehaviour, IPlayerStrategy
{
    public void Init(PlayerController controller)
    {
        
    }
    
    public bool CanMoveNext { get; private set; }

    public void OnEnter()
    {
        CanMoveNext = false;

        var minInteraction = FindCloserObject();
        
        if (minInteraction == null)
        {
            CanMoveNext = true;
            return;
        }

        CollisionInteractionUtil
            .CreateState()
            .Bind<IBOCollect>(Collect)
            .Execute(minInteraction.ContractInfo);
    }

    [CanBeNull]
    public CollisionInteraction FindCloserObject()
    {
        var colliders = Physics.OverlapSphere(transform.position, 5f);

        float minDis = Mathf.Infinity;
        CollisionInteraction minInteraction = null;
        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out CollisionInteraction interaction))
            {
                float dis = (transform.position - col.transform.position).sqrMagnitude;
                if (dis < minDis)
                {
                    minInteraction = interaction;
                    minDis = dis;
                }
            }
        }

        return minInteraction;
    }

    private void Collect(IBOCollect collect)
    {
        List<ItemData> itemList = collect.Collect();

        foreach (var item in itemList)
        {
            Debug.Log(item.ItemName);
        }

        CanMoveNext = true;

    }
}