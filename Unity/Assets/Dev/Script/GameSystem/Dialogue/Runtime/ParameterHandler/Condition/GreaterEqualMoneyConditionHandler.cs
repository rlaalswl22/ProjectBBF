using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using UnityEngine;

namespace DS.Runtime
{
    [CreateAssetMenu(menuName = "ProjectBBF/Dialogue/Condition handler/GreaterEqualMoney", fileName = "New GreaterEqualMoney")]
    public class GreaterEqualMoneyConditionHandler : ParameterHandlerArgsT<int>
    {
        protected override object OnExecute(int money)
        {
            
            var blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");

            bool isGreateEqual = blackboard.Money >= money;

            return isGreateEqual;
        }
    }
}