using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using UnityEngine;

namespace DS.Runtime
{
    [CreateAssetMenu(menuName = "ProjectBBF/Dialogue/Execution handler/Do Once", fileName = "New Do once")]
    public class DoOnceHandler : ParameterHandlerArgsT<string, bool>
    {
        protected override object OnExecute(string arg0, bool arg1)
        {
            if (string.IsNullOrEmpty(arg0)) return false;
            arg0 = arg0.Trim();

            var blackboard = PersistenceManager.Instance.LoadOrCreate<DoOnceHandlerPersistenceObject>("DoOnce");

            if (arg1)
            {
                blackboard.DoOnceList.Add(arg0);
            }
            else
            {
                blackboard.DoOnceList.Remove(arg0);
            }

            return null;
        }
    }
}