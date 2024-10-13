



using System;
using System.Collections.Generic;
using DS.Core;
using ProjectBBF.Persistence;
using UnityEngine;
using UnityEngine.AI;

public class LyllaFavorability : ActorComFavorability
{
    [SerializeField] private DialogueContainer _interruptDialogue;
    [SerializeField] private DialogueContainer _arriveDialogue;
    [SerializeField] private string _chapterKey;
    
    private const string _PERSISTENCE_KEY = "LyllaFavorability";
    

    private int _index;
    
    private LyllaFavorabilityPersistenceObject _persistenceObject;
    private ActorMove _move;

    public override void Init(Actor actor)
    {
        base.Init(actor);
        
        _move = actor.MoveStrategy;

        _persistenceObject = PersistenceManager.Instance.LoadOrCreate<LyllaFavorabilityPersistenceObject>(_PERSISTENCE_KEY);
        _index = 0;
        if (_persistenceObject._indexTable.TryGetValue(_chapterKey, out int index))
        {
            _index = index;
        }
        else
        {
            _persistenceObject._indexTable.Add(_chapterKey, 0);
        }
    }

    private void OnDestroy()
    {
        _persistenceObject._indexTable[_chapterKey] = _index;
    }

    public override DialogueEvent DequeueDialogueEvent()
    {
        IReadOnlyList<FavorabilityEventItem> events =  FavorabilityData.FavorabilityEvent.EventItems;
        
        if (_move.IsMoving)
        {
            return new DialogueEvent()
            {
                Container = _interruptDialogue,
                Type = DialogueBranchType.Dialogue
            };
        }


        if (events.Count <= _index || events.Count == 0)
        {
            return new DialogueEvent()
            {
                Container = _arriveDialogue,
                Type = DialogueBranchType.Dialogue
            };
        }

        return new DialogueEvent()
        {
            Container = events[_index++].Container,
            Type = DialogueBranchType.Dialogue
        };
    }

    public override DialogueEvent PeekDialogueEvent()
    {
        int tempIndex = _index;
        var item = DequeueDialogueEvent();
        _index = tempIndex;
        return item; 
    }
}