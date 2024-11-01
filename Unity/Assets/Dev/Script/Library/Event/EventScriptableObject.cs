using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.Events;

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

        public virtual void Raise(T arg)
        {
            OnEventRaised?.Invoke(arg);
        }

        public void Release()
        {
            OnEventRaised = null;
        }
    }
    
    //[CreateAssetMenu(menuName = "ProjectBBF/Event/..", fileName = "New eso..")]
    public abstract class EventListenerBase<TESO, TEvent>
        : MonoBehaviour
        where TEvent : IEvent
        where TESO : ESOGeneric<TEvent>
    {
        [SerializeField] private TESO _eventChannel;
        [SerializeField] private UnityEvent<TEvent> _response;

        public TESO EventChannel => _eventChannel;
        public UnityEvent<TEvent> Response => _response;

        protected virtual void OnEnable()
        {
            if (EventChannel != null)
            {
                EventChannel.OnEventRaised += OnEventRaised;
            }
        }

        protected virtual void OnDisable()
        {
            if (EventChannel != null)
            {
                EventChannel.OnEventRaised -= OnEventRaised;
            }
        }

        public virtual void OnEventRaised(TEvent evt)
        {
            Response?.Invoke(evt);
        }
    }
    public abstract class EventListenerVoidBase<TESOVoid>
        : MonoBehaviour
        where TESOVoid : ESOVoid
    {
        [SerializeField] private TESOVoid _eventChannel;
        [SerializeField] private UnityEvent<TESOVoid> _response;
        [SerializeField] private List<ESOVoid> _esoChain;

        public TESOVoid EventChannel => _eventChannel;
        public UnityEvent<TESOVoid> Response => _response;

        protected virtual void OnEnable()
        {
            if (EventChannel != null)
            {
                EventChannel.OnEventRaised += OnEventRaised;
            }
        }

        protected virtual void OnDisable()
        {
            if (EventChannel != null)
            {
                EventChannel.OnEventRaised -= OnEventRaised;
            }
        }

        public void OnEventRaised()
        {
            Response?.Invoke(EventChannel);

            foreach (var eso in _esoChain)
            {
                if (eso)
                {
                    eso.Raise();
                }
            }
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