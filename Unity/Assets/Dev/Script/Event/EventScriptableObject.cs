using System;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using UnityEngine;

namespace ProjectBBF.Event
{
    public abstract class EventScriptableObject : ScriptableObject
    {
        public abstract void Release();
    }

    public abstract class EventScriptableObjectT<T1> : EventScriptableObject
    {
        public event Action<T1> OnSignal;

        private UniTaskCompletionSource<T1> _tcs;

        public UniTaskCompletionSource<T1> Tcs
        {
            get
            {
                if (_tcs is null)
                {
                    _tcs = new UniTaskCompletionSource<T1>();
                }

                return _tcs;
            }
        }

        public bool IsTriggered { get; set; }

        public void Signal(T1 arg)
        {
            if (Tcs.TrySetResult(arg) is false)
            {
                Debug.LogError($"'{name}' 에러");
            }

            OnSignal?.Invoke(arg);
        }

        public UniTask<T1> WaitAsync()
        {
            return Tcs.Task;
        }

        public override void Release()
        {
            _tcs = null;
            IsTriggered = false;
        }
    }

    public abstract class EventScriptableObjectT<T1, T2> : EventScriptableObject
    {
        public event Action<T1, T2> OnSignal;

        private UniTaskCompletionSource<(T1, T2)> _tcs;

        public UniTaskCompletionSource<(T1, T2)> Tcs
        {
            get
            {
                if (_tcs is null)
                {
                    _tcs = new UniTaskCompletionSource<(T1, T2)>();
                }

                return _tcs;
            }
        }

        public bool IsTriggered { get; set; }

        public void Signal(T1 arg1, T2 arg2)
        {
            if (Tcs.TrySetResult((arg1, arg2)) is false)
            {
                Debug.LogError($"'{name}' 에러");
            }

            OnSignal?.Invoke(arg1, arg2);
        }

        public UniTask<(T1, T2)> WaitAsync()
        {
            return Tcs.Task;
        }

        public override void Release()
        {
            _tcs = null;
            IsTriggered = false;
        }
    }

    public abstract class EventScriptableObjectT<T1, T2, T3> : EventScriptableObject
    {
        public event Action<T1, T2, T3> OnSignal;

        private UniTaskCompletionSource<(T1, T2, T3)> _tcs;

        public UniTaskCompletionSource<(T1, T2, T3)> Tcs
        {
            get
            {
                if (_tcs is null)
                {
                    _tcs = new UniTaskCompletionSource<(T1, T2, T3)>();
                }

                return _tcs;
            }
        }

        public bool IsTriggered { get; set; }

        public void Signal(T1 arg1, T2 arg2, T3 arg3)
        {
            if (Tcs.TrySetResult((arg1, arg2, arg3)) is false)
            {
                Debug.LogError($"'{name}' 에러");
            }

            OnSignal?.Invoke(arg1, arg2, arg3);
        }

        public UniTask<(T1, T2, T3)> WaitAsync()
        {
            return Tcs.Task;
        }

        public override void Release()
        {
            _tcs = null;
            IsTriggered = false;
        }
    }

    public abstract class EventScriptableObjectT<T1, T2, T3, T4> : EventScriptableObject
    {
        public event Action<T1, T2, T3, T4> OnSignal;

        private UniTaskCompletionSource<(T1, T2, T3, T4)> _tcs;

        public UniTaskCompletionSource<(T1, T2, T3, T4)> Tcs
        {
            get
            {
                if (_tcs is null)
                {
                    _tcs = new UniTaskCompletionSource<(T1, T2, T3, T4)>();
                }

                return _tcs;
            }
        }

        public bool IsTriggered { get; set; }

        public void Signal(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (Tcs.TrySetResult((arg1, arg2, arg3, arg4)) is false)
            {
                Debug.LogError($"'{name}' 에러");
            }

            OnSignal?.Invoke(arg1, arg2, arg3, arg4);
        }

        public UniTask<(T1, T2, T3, T4)> WaitAsync()
        {
            return Tcs.Task;
        }

        public override void Release()
        {
            _tcs = null;
            IsTriggered = false;
        }
    }

    public abstract class EventScriptableObjectT<T1, T2, T3, T4, T5> : EventScriptableObject
    {
        public event Action<T1, T2, T3, T4, T5> OnSignal;

        private UniTaskCompletionSource<(T1, T2, T3, T4, T5)> _tcs;

        public UniTaskCompletionSource<(T1, T2, T3, T4, T5)> Tcs
        {
            get
            {
                if (_tcs is null)
                {
                    _tcs = new UniTaskCompletionSource<(T1, T2, T3, T4, T5)>();
                }

                return _tcs;
            }
        }

        public bool IsTriggered { get; set; }

        public void Signal(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (Tcs.TrySetResult((arg1, arg2, arg3, arg4, arg5)) is false)
            {
                Debug.LogError($"'{name}' 에러");
            }

            OnSignal?.Invoke(arg1, arg2, arg3, arg4, arg5);
        }

        public UniTask<(T1, T2, T3, T4, T5)> WaitAsync()
        {
            return Tcs.Task;
        }

        public override void Release()
        {
            _tcs = null;
            IsTriggered = false;
        }
    }
}