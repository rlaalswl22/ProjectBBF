using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DS.Core;
using DS.Runtime;
using MyBox;
using ProjectBBF.Event;
using ProjectBBF.Persistence;
using UnityEngine;

[GameData]
[Serializable]
public class MapCutSceneEventPersistence
{
    public bool IsPlayed;
    [NonSerialized, DoNotEditable] public bool IsPlayedCutScene;
}

public class MapCutSceneEventReceiver : MonoBehaviour
{
    [SerializeField] private MapTriggerBase _trigger;
    [SerializeField] private string _mapEventKey;

    [SerializeField] private SceneName _scene;
    [SerializeField] private DialogueContainer _beforeContainer;
    [SerializeField] private DialogueContainer _afterContainer;

    [SerializeField] private bool _playOnce = true;

    [SerializeField] private string _fadeinDirectorKey;
    [SerializeField] private string _fadeoutDirectorKey;
    [SerializeField] private Vector2 _rollbackPos;

    private PlayerController _playerController;

    private PlayerController PlayerController
    {
        get
        {
            if (_playerController == false)
            {
                GameObjectStorage.Instance.StoredObjects.ForEach(x =>
                {
                    if (x.CompareTag("Player") && x.TryGetComponent(out PlayerController com))
                    {
                        _playerController = com;
                    }
                });
            }

            return _playerController;
        }
    }

    private void Awake()
    {
        if (_trigger)
        {
            _trigger.OnTrigger += Play;
        }
        SceneLoader.Instance.FadeinComplete += OnFadein;
    }

    private void OnDestroy()
    {
        if (_trigger)
        {
            _trigger.OnTrigger -= Play;
        }
        
        if (SceneLoader.Instance)
        {
            SceneLoader.Instance.FadeinComplete -= OnFadein;
        }
    }

    private void OnFadein()
    {
        var persistence =
            PersistenceManager.Instance.LoadOrCreate<MapCutSceneEventPersistence>(_mapEventKey);

        if (persistence.IsPlayed && _playOnce)
        {
            SceneLoader.Instance.FadeinComplete -= OnFadein;
            return;
        }

        if (persistence.IsPlayedCutScene)
        {

            if (_afterContainer)
            {
                PlayerController.StateHandler.TranslateState("ToDialogue");
                _ = PlayerController.Dialogue.RunDialogue(_afterContainer, ProcessorData.Default).ContinueWith(_ =>
                {
                    PlayerController.StateHandler.TranslateState("EndOfDialogue");
                });
            }
            else
            {
                PlayerController.StateHandler.TranslateState("EndOfCutScene");
            }

            persistence.IsPlayedCutScene = false;
            persistence.IsPlayed = true;
        }
    }

    public void Play(CollisionInteractionMono caller)
    {
        if ((caller.Owner as PlayerController) == false) return;
        
        _playerController = caller.Owner as PlayerController;
        
        var persistence =
            PersistenceManager.Instance.LoadOrCreate<MapCutSceneEventPersistence>(_mapEventKey);

        if (persistence.IsPlayedCutScene) return;
        if (persistence.IsPlayed && _playOnce) return;

        var loaderInst = SceneLoader.Instance;
        var persistenceInst = PersistenceManager.Instance;
        var blackboard = persistenceInst.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");


        blackboard.CurrentWorld = loaderInst.CurrentWorldScene;
        blackboard.CurrentPosition = _rollbackPos;


        persistence.IsPlayedCutScene = true;

        if (PlayerController)
        {

            if (_beforeContainer)
            {
                PlayerController.StateHandler.TranslateState("ToDialogue");
                _ = PlayerController.Dialogue.RunDialogue(_beforeContainer, ProcessorData.Default)
                    .ContinueWith(s =>LoadScene(_playerController));
            }
            else
            {
                LoadScene(_playerController);
            }
        }
    }

    private void LoadScene(PlayerController pc)
    {
        var loaderInst = SceneLoader.Instance;
        PlayerController.StateHandler.TranslateState("ToCutScene");
        TimeManager.Instance.Pause();
        var xy = pc.Blackboard.CurrentPosition;
        
        _ = loaderInst
                .WorkDirectorAsync(false, _fadeoutDirectorKey)
                .ContinueWith(_ => pc.transform.SetXY(xy))
                .ContinueWith(() => SceneLoader.Instance.LoadWorldAsync(_scene))
                .ContinueWith(_ => SceneLoader.Instance.WorkDirectorAsync(true, _fadeoutDirectorKey))
            ;
    }
    
}