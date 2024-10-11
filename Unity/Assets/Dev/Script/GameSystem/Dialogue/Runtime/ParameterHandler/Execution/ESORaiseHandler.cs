using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using ProjectBBF.Persistence;
using UnityEngine;

namespace DS.Runtime
{
    [CreateAssetMenu(menuName = "ProjectBBF/Dialogue/Execution handler/ESO Raise", fileName = "New ESORaise")]
    public class ESORaiseHandler : ParameterHandlerArgsT<ESOVoid>
    {

        protected override object OnExecute(ESOVoid eso)
        {
            if (eso == false) return null;
            
            eso.Raise();
            
            return null;
        }
    }
}