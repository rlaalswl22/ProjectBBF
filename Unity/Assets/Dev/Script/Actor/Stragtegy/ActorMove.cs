using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Cysharp.Threading.Tasks;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.AI;

public class ActorMove : MonoBehaviour, IActorStrategy
{
    private NavMeshAgent _agent;
    private ActorMovementData _data;
    private Actor _actor;

    private CancellationTokenSource _moveCancel;
    
    public CollisionInteraction Interaction { get; private set; }
    public void Init(Actor actor)
    {
        _agent = actor.GetComponent<NavMeshAgent>();
        _data = actor.MovementData;
        _actor = actor;
        Interaction = actor.Interaction;
        
        _moveCancel = CancellationTokenSource.CreateLinkedTokenSource(GlobalCancelation.PlayMode);
    }

    public void ResetMove()
    {
        _moveCancel?.Cancel();
        _moveCancel = CancellationTokenSource.CreateLinkedTokenSource(GlobalCancelation.PlayMode);
    }
    
    public async UniTask<Vector2> MoveToPoint(PatrolPoint point)
    {
        await UniTask.WaitUntil(() =>
        {
            if (_agent == false) return true;
            if (TimeManager.Instance is not null && TimeManager.Instance.IsRunning is false)
            {
                _agent.isStopped = true;
                return false;
            }
            
            
            _agent.isStopped = false;

            Vector3 pos = point.Position;
            if (point.InteractiveDecoratedPoint)
            {
                pos = point.InteractiveDecoratedPoint.InteractingPosition;
                if (point.InteractiveDecoratedPoint.Teleport)
                {
                    _agent.transform.position = (Vector2)point.Position;
                    _agent.SetDestination(point.Position);
                    _actor.Visual.LookAt(_agent.desiredVelocity, false);
                    return true;
                }   
            }
            
            _agent.speed = _data.MovementSpeed;
            _agent.SetDestination(pos);
            _actor.Visual.LookAt(_agent.desiredVelocity, false);
            return Vector2.Distance(_agent.transform.position, pos) <= _agent.stoppingDistance;
        }, PlayerLoopTiming.Update, _moveCancel.Token);

        bool backupVisible = _actor.Visual.IsVisible;
        _agent.isStopped = false;
        await InteractPoint(point);
        _actor.Visual.IsVisible = backupVisible;

        return point.Position;
    }

    private async UniTask InteractPoint(PatrolPoint point)
    {
        var decoPoint = point.InteractiveDecoratedPoint;
        if (decoPoint == false) return;

        _actor.Visual.IsVisible = !decoPoint.VisitAndHide;

        _actor.Visual.LookAt(_agent.desiredVelocity, true);
        float timer = 0f;
        while (timer < decoPoint.WaitDuration)
        {
            if (_moveCancel.IsCancellationRequested)
            {
                return;
            }
            if (TimeManager.Instance is not null && TimeManager.Instance.IsRunning is false)
            {
                await UniTask.Yield();
                continue;
            }

            timer += Time.deltaTime;
            await UniTask.Yield();
        }


    }

}