using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using UnityEngine;

namespace DS.Runtime
{
    [CreateAssetMenu(menuName = "ProjectBBF/Dialogue/Condition handler/Quest", fileName = "New Quest condition")]
    public class QuestConditionHandler : ParameterHandlerArgsT<string, int>
    {
        protected override object OnExecute(string arg0, int arg1)
        {
            if (QuestManager.Instance)
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
                    case 3:
                        return QuestManager.Instance.GetQuestState(arg0) is QuestType.Cancele or QuestType.Complete;
                    case 4:
                        type = QuestType.NotExist;
                        break;
                    default:
                        return false;
                }
                
                return type == QuestManager.Instance.GetQuestState(arg0);
            }
            
            return false;
        }
    }
}