using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DS.Runtime;
using MyBox;
using ProjectBBF.Persistence;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueCutSceneController : MonoBehaviour, INotificationReceiver
{
    [SerializeField] private string _fadeinDirectorKey;
    [SerializeField] private string _fadeoutDirectorKey;
    [SerializeField] private PlayableDirector _director;

    private void Awake()
    {
        _director.stopped += OnStopped;
    }
    private void OnDestroy()
    {
        _director.stopped -= OnStopped;
    }

    private void OnStopped(PlayableDirector obj)
    {
        var loaderInst = SceneLoader.Instance;
        var persistenceInst = PersistenceManager.Instance;

        var blackboard = persistenceInst.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");
        string scene = blackboard.CurrentWorld;
        Vector2 pos = blackboard.CurrentPosition;

        _ = loaderInst
            .WorkDirectorAsync(false, _fadeoutDirectorKey)
            .ContinueWith(_ => loaderInst.LoadImmutableScenesAsync())
            .ContinueWith(_ => loaderInst.LoadWorldAsync(scene))
            .ContinueWith(_ =>
            {
                GameObjectStorage.Instance.StoredObjects.ForEach(x =>
                {
                    if (x.CompareTag("Player") && x.TryGetComponent(out PlayerController pc))
                    {
                        x.transform.position = pos;
                        pc.StateHandler.TranslateState("EndOfDoNothing");
                    }
                });
            })
            .ContinueWith(() => loaderInst.WorkDirectorAsync(true, _fadeinDirectorKey))
            .ContinueWith(_ => TimeManager.Instance.Resume())
            ;
    }


    public void Play()
    {
        _director.Play();
    }

    public void Stop()
    {
        _director.Stop();
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is DialogueMarker dialogueMarker)
        {
            _ = dialogueMarker.OnPlay(_director, this.GetCancellationTokenOnDestroy());
        }
        else if (notification is BranchTimelineMarker)
        {
            var timelineAsset = TimelineAssetHandler.TimelineAsset;

            if (timelineAsset == false)
            {
                _director.Stop();
                return;
            }
            
            _director.playableAsset = timelineAsset;
            TimelineAssetHandler.TimelineAsset = null;
            _director.time = 0;
            _director.Play();
        }
        else if (notification is DirectorTimelineMarker directorMarker)
        {
            _ = directorMarker.OnPlay(_director);
        }

    }
}