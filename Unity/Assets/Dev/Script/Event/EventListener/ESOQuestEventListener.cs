using System;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.Events;

public class ESOQuestEventListener : EventListenerBase<ESOQuest, QuestEvent>
{
    [SerializeField] private UnityEvent _onComplete;
    [SerializeField] private UnityEvent _onCreate;
    [SerializeField] private UnityEvent _onCancel;

    public override void OnEventRaised(QuestEvent evt)
    {
        base.OnEventRaised(evt);
        
        switch(evt.Type)
        {
            case QuestType.Create:
                _onCreate?.Invoke();
                break;
            case QuestType.Complete:
                _onComplete?.Invoke();
                break;
            case QuestType.Cancele:
                _onCancel?.Invoke();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}