using System;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using UnityEngine;

namespace DS.Runtime
{
    [CreateAssetMenu(menuName = "ProjectBBF/Dialogue/Execution handler/Add Item", fileName = "New Add item")]
    public class AddItemHandler : ParameterHandlerArgsT<ItemData, int>
    {
        protected override object OnExecute(ItemData arg0, int arg1)
        {
            if (arg0 == false) return false;
            if (arg1 == 0) return false;

            var blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");

            blackboard.Inventory.Model.PushItem(arg0, arg1);

            return null;
        }
    }
}