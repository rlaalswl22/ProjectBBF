using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using ProjectBBF.Persistence;
using UnityEngine;

public enum QuestType
{
    Create,
    Complete,
    Cancele,
}
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
        base.Raise(arg);

        if (PersistenceManager.Instance)
        {
            var obj = PersistenceManager.Instance.LoadOrCreate<QuestPersistence>(QuestManager.PERSISTENCE_KEY);
            obj.QuestTable[arg.QuestKey] = arg.Type;
        }
    }
}