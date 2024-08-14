using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;

public class ActorMove : MonoBehaviour, IActorStrategy, IBAMove
{
    private Rigidbody2D _rigid;
    private ActorMovementData _data;
    
    public CollisionInteraction Interaction { get; private set; }
    public bool MoveLock { get; set; }
    public void Init(Actor actor)
    {
        _rigid = actor.Rigid;
        _data = actor.MovementData;
        Interaction = actor.Interaction;
    }
    
    public async UniTask<Vector2> MoveToPoint(Vector2 pos)
    {
        await UniTask.WaitUntil(() =>
        {
            _rigid.position = Vector2.MoveTowards(_rigid.position, pos, Time.deltaTime * _data.MovementSpeed);

            return Vector2.Distance(_rigid.position, pos) < 0.001f;
        }, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);

        return pos;
    }

}