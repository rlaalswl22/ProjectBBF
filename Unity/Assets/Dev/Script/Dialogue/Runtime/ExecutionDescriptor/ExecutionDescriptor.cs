using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DS.Runtime
{
    public abstract class ExecutionDescriptor : ScriptableObject
    {
        public abstract Type[] GetArgumentTypes();

        public void Execute(object[] args)
        {
            var types = GetArgumentTypes();
            for (int i = 0; i < Mathf.Min(args.Length, types.Length); i++)
            {
                if (args[i].GetType() != types[i])
                {
                    Debug.LogError("올바르지 않은 타입");
                    return;
                }
            }

            // 제너릭 메서드를 찾는 로직
            var method = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name == "OnExecute" &&
                                     m.GetParameters().Select(p => p.ParameterType).SequenceEqual(types));
            if (method != null)
            {
                method.Invoke(this, args);
            }
            else
            {
                Debug.LogError("OnExecute method not found");
            }
        }
    }

    public abstract class ExecutionDescriptorT : ExecutionDescriptor
    {
        public sealed override Type[] GetArgumentTypes()
            => new Type[]{};
    }
    public abstract class ExecutionDescriptorT<T0> : ExecutionDescriptor
    {
        public sealed override Type[] GetArgumentTypes()
            => new []{ typeof(T0), };
        
        protected abstract void OnExecute(T0 arg0);
    }
    public abstract class ExecutionDescriptorT<T0, T1> : ExecutionDescriptor
    {
        public sealed override Type[] GetArgumentTypes()
            => new []{ typeof(T0), typeof(T1), };
        
        protected abstract void OnExecute(T0 arg0, T1 arg1);
    }
    public abstract class ExecutionDescriptorT<T0, T1, T2> : ExecutionDescriptor
    {
        public sealed override Type[] GetArgumentTypes()
            => new []{ typeof(T0), typeof(T1), typeof(T2)};
        
        protected abstract void OnExecute(T0 arg0, T1 arg1, T2 arg2);
    }
    public abstract class ExecutionDescriptorT<T0, T1, T2, T3> : ExecutionDescriptor
    {
        public sealed override Type[] GetArgumentTypes()
            => new []{ typeof(T0), typeof(T1), typeof(T2), typeof(T3)};
        
        protected abstract void OnExecute(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    }
    public abstract class ExecutionDescriptorT<T0, T1, T2, T3, T4> : ExecutionDescriptor
    {
        public sealed override Type[] GetArgumentTypes()
            => new []{ typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4)};
        
        protected abstract void OnExecute(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }
}