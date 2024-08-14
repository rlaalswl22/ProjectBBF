using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StateTransitionHandler : MonoBehaviour
{
    public delegate bool Callback();

    private Dictionary<string, Callback> _callbackTable = new();
    
    public bool AddHandleCallback(string targetStateKey, Callback callback)
    {
        Debug.Assert(callback is not null);
        Debug.Assert(string.IsNullOrEmpty(targetStateKey) == false);

        return _callbackTable.TryAdd(targetStateKey, callback);
    }

    public bool RemoveHandleCallback(string targetStateKey)
    {
        Debug.Assert(string.IsNullOrEmpty(targetStateKey) == false);

        return _callbackTable.Remove(targetStateKey);
    }

    public bool CanTransfer(string targetStateKey)
    {
        Debug.Assert(string.IsNullOrEmpty(targetStateKey) == false);

        if (_callbackTable.TryGetValue(targetStateKey, out var callback))
        {
            return callback();
        }

        return false;
    }
}