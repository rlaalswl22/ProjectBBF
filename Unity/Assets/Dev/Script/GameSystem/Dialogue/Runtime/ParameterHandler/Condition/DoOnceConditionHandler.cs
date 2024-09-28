using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using UnityEngine;

namespace DS.Runtime
{
    [CreateAssetMenu(menuName = "ProjectBBF/Dialogue/Condition handler/Do Once", fileName = "New Do once")]
    public class DoOnceConditionHandler : ParameterHandlerArgsT<string>
    {
        protected override object OnExecute(string arg0)
        {
            if (string.IsNullOrEmpty(arg0)) return false;
            arg0 = arg0.Trim();
            
            var blackboard = PersistenceManager.Instance.LoadOrCreate<DoOnceHandlerPersistenceObject>("DoOnce");

            bool isContains = blackboard.DoOnceList.Contains(arg0);
            return isContains;
        }
    }
}