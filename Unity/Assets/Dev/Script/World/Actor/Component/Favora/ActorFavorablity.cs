using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DS.Core;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;

public class ActorFavorablity: ActorComponent, IBADialogue
{
    private Actor _actor;
    
    public CollisionInteraction Interaction => _actor.Interaction;

    public FavorablityContainer FavorablityContainer { get; private set; }
    
    public DialogueEvent DequeueDialogueEvent()
    {
        // TODO: 테스트 코드
        if (FavorablityContainer.Event.EventItems.Count == 0) return DialogueEvent.Empty;

        FavorabilityEventItem eventItem = FavorablityContainer.Event.EventItems[0];
        return new DialogueEvent()
        {
            Container = eventItem.Container,
            Type = eventItem.BranchType
        };
    }

    public DialogueEvent PeekDialogueEvent()
    {
        // TODO: 테스트 코드
        if (FavorablityContainer.Event.EventItems.Count == 0) return DialogueEvent.Empty;

        return new DialogueEvent()
        {
            Container = FavorablityContainer.Event.EventItems[0].Container,
            Type = DialogueBranchType.Dialogue | DialogueBranchType.Exit
        };
    }

    public override void Init(Actor actor)
    {
        _actor = actor;
        
        if (ActorDataManager.Instance.CachedDict.TryGetValue(actor.ActorKey, out var data))
        {
            FavorablityContainer = new FavorablityContainer(data.FavorabilityEvent, 0, null);
        }
        else
        {
            Debug.LogError($"actor key({actor.ActorKey})를 찾을 수 없음");
        }
    }
}