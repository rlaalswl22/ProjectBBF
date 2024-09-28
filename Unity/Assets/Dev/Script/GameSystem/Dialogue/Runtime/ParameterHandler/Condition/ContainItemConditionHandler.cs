using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using UnityEngine;

namespace DS.Runtime
{
    [CreateAssetMenu(menuName = "ProjectBBF/Dialogue/Condition handler/Contain Item", fileName = "New Contain item")]
    public class ContainItemConditionHandler : ParameterHandlerArgsT<ItemData, int>
    {
        protected override object OnExecute(ItemData arg0, int arg1)
        {
            if (arg0 == false) return false;
            if (arg1 == 0) return false;

            var blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");

            bool isContains = blackboard.Inventory.Model.ContainsGreaterEqual(arg0, arg1);

            return isContains;
        }
    }
}