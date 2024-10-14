using System;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using UnityEngine;

namespace DS.Runtime
{
    [CreateAssetMenu(menuName = "ProjectBBF/Dialogue/Execution handler/Add Money", fileName = "New Add Money")]
    public class AddMoneyHandler : ParameterHandlerArgsT<int>
    {
        protected override object OnExecute(int money)
        {
            var blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");

            blackboard.Money += money;

            return null;
        }
    }
}