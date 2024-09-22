using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class MinigameBase : MonoBehaviour
{
    [SerializeField] private MinigameData _data;

    public MinigameData Data => _data;

    protected virtual void Awake()
    {
        MinigameController.Instance.OnSignalMinigameStart += OnStart;
        MinigameController.Instance.OnSignalMinigameEnd += OnEnd;
    }

    protected virtual void OnDestroy()
    {
            MinigameController.Instance.OnSignalMinigameStart -= OnStart;
        MinigameController.Instance.OnSignalMinigameEnd -= OnEnd;
    }

    private void OnStart(string obj)
    {
        if (_data.MinigameKey != obj) return;

        MinigameController.Instance.CurrentGameKey = _data.MinigameKey;
        
        OnSignal();
        _ = OnSignalAsync();
    }

    private void OnEnd(string obj)
    {
        if (_data.MinigameKey != obj) return;

        if (MinigameController.Instance.CurrentGameKey == _data.MinigameKey)
        {
            MinigameController.Instance.CurrentGameKey = null;
        }

        OnEndSignal();
        _ = OnEndSignalAsync();
    }

    protected void Release()
    {
        if (MinigameController.Instance.CurrentGameKey == _data.MinigameKey)
        {
            MinigameController.Instance.CurrentGameKey = null;
        }
    }
    
    protected virtual void OnSignal()
    {
    }

    protected virtual UniTask OnSignalAsync()
    {
        return UniTask.CompletedTask;
    }
    
    protected virtual void OnEndSignal()
    {
    }

    protected virtual UniTask OnEndSignalAsync()
    {
        return UniTask.CompletedTask;
    }
}

public abstract class MinigameData : ScriptableObject
{
    [SerializeField] private string _minigameKey;
    [SerializeField] private string _directorKey;

    public string MinigameKey => _minigameKey;
    public string DirectorKey => _directorKey;
}