using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/InteractiveObject/Behaviour/HandCollecting", fileName = "HandCollecting")]
public class HandCollectingBehaviour : CollectingObjectBehaviour, IBOCollect
{
    [Serializable]
    private struct ItemSet
    {
        [field: SerializeField, OverrideLabel("아이템 데이터"), InitializationField, MustBeAssigned]
        public CountableItemData Data;

        [field: SerializeField, OverrideLabel("아이템 개수"), InitializationField, MustBeAssigned, MinValue(1)]
        public int Count;
    }

    [field: SerializeField, Header("드랍 아이템 목록"), InitializationField, MustBeAssigned]
    private List<ItemSet> _itemList;

    public CollisionInteraction Interaction { get; private set; }

    private CollectingObjectData _data;

    public async UniTask<List<Item>> Collect()
    {
        await UniTask.Delay((int)(_data.CollectingTime * 1000f));


        List<Item> tempList = new List<Item>(2);
        return _itemList
            .SelectMany(x =>
            {
                tempList.Clear();
                for (int i = 0; i < x.Count; i++)
                {
                    tempList.Add(new CountableItem(x.Data, 1) as Item);
                }

                return tempList;
            })
            .ToList();
    }

    public override void InitBehaviour(CollectingObjectData data, CollisionInteraction interaction,
        ObjectContractInfo info)
    {
        Interaction = interaction;
        _data = data;

        info.AddBehaivour<IBOCollect>(this);
    }
}