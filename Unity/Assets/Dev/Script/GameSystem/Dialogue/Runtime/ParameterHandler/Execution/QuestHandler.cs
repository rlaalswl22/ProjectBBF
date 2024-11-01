using System;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using UnityEngine;

namespace DS.Runtime
{
    [CreateAssetMenu(menuName = "ProjectBBF/Dialogue/Execution handler/Quest", fileName = "New Quest")]
    public class QuestHandler : ParameterHandlerArgsT<string, int>
    {
        protected override object OnExecute(string arg0, int arg1)
        {
            QuestType type;
                
            switch (arg1)
            {
                case 0:
                    type = QuestType.Complete;
                    break;
                case 1:
                    type = QuestType.Create;
                    break;
                case 2:
                    type = QuestType.Cancele;
                    break;
                default:
                    return false;
            }
            
            QuestManager.Instance.ESO.Raise(new QuestEvent()
            {
                QuestKey = arg0,
                Type = type
            });
            return null;
        }
    }
}