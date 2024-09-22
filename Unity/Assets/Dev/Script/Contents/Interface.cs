using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DS.Core;
using UnityEngine;

public abstract class MinigameBase : MonoBehaviour
{
    [SerializeField] private MinigameData _data;

    public MinigameData Data => _data;

    protected PlayerController Player { get; private set; }
    private bool _isRequestEnd;
    
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

        if (Player == false)
        {
            foreach (var storedObject in GameObjectStorage.Instance.StoredObjects)
            {
                if (storedObject.CompareTag("Player") && storedObject.TryGetComponent(out PlayerController pc))
                {
                    Player = pc;
                    break;
                }
            }
        }
        
        MinigameController.Instance.CurrentGameKey = _data.MinigameKey;

        _ = OnSignalAsync();
    }

    private void OnEnd(string obj)
    {
        if (_data.MinigameKey != obj) return;

        if (MinigameController.Instance.CurrentGameKey == _data.MinigameKey)
        {
            MinigameController.Instance.CurrentGameKey = null;
        }

        _isRequestEnd = true;
    }

    protected void Release()
    {
        if (MinigameController.Instance.CurrentGameKey == _data.MinigameKey)
        {
            MinigameController.Instance.CurrentGameKey = null;
        }
        
        _isRequestEnd = false;
    }

    private async UniTask OnSignalAsync()
    {
        try
        {
            DialogueController.Instance.ResetDialogue();
            Player.StateHandler.TranslateState("EndOfInteractionDialogue");
            Player.StateHandler.TranslateState("ToDialogue");
            await SceneLoader.Instance.WorkDirectorAsync(false, Data.DirectorKey);
            OnGameInit();
            await SceneLoader.Instance.WorkDirectorAsync(true, Data.DirectorKey);
            Player.StateHandler.TranslateState("EndOfDialogue");

            await OnTutorial();

            OnGameBegin();

            while (IsGameEnd() is false && _isRequestEnd is false)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
            }

            if (_isRequestEnd)
            {
                Player.StateHandler.TranslateState("EndOfInteractionDialogue");
            }
            Player.StateHandler.TranslateState("ToDialogue");
            await SceneLoader.Instance.WorkDirectorAsync(false, Data.DirectorKey);
            OnGameRelease();
            await SceneLoader.Instance.WorkDirectorAsync(true, Data.DirectorKey);
            Player.StateHandler.TranslateState("EndOfDialogue");

            if (_isRequestEnd)
            {
                await RunDialogue(Data.DialogueAfterGameEnd);
            }
            else
            {
                await RunDialogue(Data.DialogueAfterGameExit);
            }

            Release();
        }
        catch (Exception e)when (e is OperationCanceledException)
        {
            Debug.LogException(e);
            throw;
        }
       
    }

    protected UniTask RunDialogue(DialogueContainer container)
    {
        DialogueController.Instance.ResetDialogue();
        Player.StateHandler.TranslateState("ToDialogue");
        return Player.Dialogue.RunDialogue(container).ContinueWith(_ =>
        {
            Player.StateHandler.TranslateState("EndOfDialogue");
        });
    }

    protected abstract void OnGameInit();
    protected abstract UniTask OnTutorial();
    protected abstract void OnGameBegin();
    protected abstract void OnGameRelease();
    protected abstract bool IsGameEnd();
    
}

public abstract class MinigameData : ScriptableObject
{
    [SerializeField] private string _minigameKey;
    [SerializeField] private string _directorKey;

    [SerializeField] private DialogueContainer _dialogueAfterGameEnd;
    [SerializeField] private DialogueContainer _dialogueAfterGameExit;
    public string MinigameKey => _minigameKey;
    public string DirectorKey => _directorKey;

    public DialogueContainer DialogueAfterGameEnd => _dialogueAfterGameEnd;
    public DialogueContainer DialogueAfterGameExit => _dialogueAfterGameExit;
}