


using System;
using System.Collections.Generic;
using UnityEngine;

public class LyllaMove : ActorComponent
{
    [SerializeField] private List<ESOLyllaPath> _events;
    private ActorMove _move;
    private Actor _owner;
    
    private List<Action> _eventActions = new List<Action>();
    
    public void Init(Actor actor)
    {
        _move = actor.MoveStrategy;
        _owner = actor;
    }

    private void OnEnable()
    {
        foreach (var eso in _events)
        {
            if(eso == false)continue;
            
            _eventActions.Add(() => OnRaised(eso));
            eso.OnEventRaised += _eventActions[^1];
        }
    }
    private void OnDisable()
    {
        int i = 0;
        foreach (var eso in _events)
        {
            if(eso == false)continue;
            
            eso.OnEventRaised -= _eventActions[i++];
        }
        
        _eventActions.Clear();
    }

    private void OnRaised(ESOLyllaPath eso)
    {
        _owner.PatrolPath = eso.PatrolPointPath;
        _move.ResetMove();
    }
}