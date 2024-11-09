using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using ProjectBBF.Persistence;
using UnityEngine;

public struct QuestEvent : IEvent
{
    public string QuestKey;
    public QuestType Type;
}

[CreateAssetMenu(menuName = "ProjectBBF/Event/Quest", fileName = "New Quest")]
public class ESOQuest : ESOGeneric<QuestEvent>
{
    public override void Raise(QuestEvent arg)
    {
        if (QuestManager.Instance)
        {
            bool success = QuestManager.Instance.ChangeQuestState(arg.QuestKey, arg.Type);

            if (success)
            {
                base.Raise(arg);
            }
        }
        
    }
}