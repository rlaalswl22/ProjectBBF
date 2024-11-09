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
            var obj = PersistenceManager.Instance.LoadOrCreate<QuestPersistence>(QuestManager.PERSISTENCE_KEY);

            if (obj.QuestTable.TryGetValue(arg0, out var value))
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
                        return value is QuestType.Cancele or QuestType.Complete;
                    case 4: // 퀘스트가 발행됐는지 검사
                        return true;
                    default:
                        return false;
                }
                
                return type == value;
            }
            
            return false;
        }
    }
}