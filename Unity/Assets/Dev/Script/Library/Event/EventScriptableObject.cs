using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using UnityEngine;

namespace ProjectBBF.Event
{
    public interface IEvent
    {
    }
    public abstract class EventScriptableObject : ScriptableObject
    {
        public abstract void Release();
    }
    public abstract class ESOGeneric<T> : ScriptableObject
        where T : IEvent
    {
        public event Action<T> OnEventRaised;

        public void Raise(T arg)
        {
            OnEventRaised?.Invoke(arg);
        }

        public void Release()
        {
            OnEventRaised = null;
        }
    }
    
    public abstract class AsyncESO<T1> : EventScriptableObject
    {
        public event Action<T1> OnSignal;
        private T1 _arg1;
        public bool IsTriggered { get; set; }

        public void Signal(T1 arg1)
        {
            _arg1 = arg1;
            IsTriggered = true;
            OnSignal?.Invoke(arg1);
        }

        public async UniTask<T1> WaitAsync(CancellationToken token = default)
        {
            while (IsTriggered is false && token.IsCancellationRequested is false)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            if (token.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            await UniTask.Yield(PlayerLoopTiming.Update, token);
            return _arg1;
        }

        public override void Release()
        {
            IsTriggered = false;
            _arg1 = default;
        }
    }
}