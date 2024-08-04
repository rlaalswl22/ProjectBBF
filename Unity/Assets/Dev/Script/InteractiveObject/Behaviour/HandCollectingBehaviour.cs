using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/InteractiveObject/Behaviour/HandCollecting", fileName = "HandCollecting")]
public class HandCollectingBehaviour : CollectingObjectBehaviour, IBOCollect
{
    [Serializable]
    private struct ItemSet
    {
        [field: SerializeField, OverrideLabel("아이템 데이터"), InitializationField, MustBeAssigned]
        public ItemData Data;

        [field: SerializeField, OverrideLabel("아이템 개수"), InitializationField, MustBeAssigned, MinValue(1)]
        public int Count;
    }

    [field: SerializeField, Header("드랍 아이템 목록"), InitializationField, MustBeAssigned]
    private List<ItemSet> _itemList;

    public CollisionInteraction Interaction { get; private set; }

    private CollectingObjectData _data;

    public List<ItemData> Collect()
    {
        return new List<ItemData>();
    }

    public override void InitBehaviour(CollectingObjectData data, CollisionInteraction interaction,
        ObjectContractInfo info)
    {
        Interaction = interaction;
        _data = data;

        info.AddBehaivour<IBOCollect>(this);
    }
}