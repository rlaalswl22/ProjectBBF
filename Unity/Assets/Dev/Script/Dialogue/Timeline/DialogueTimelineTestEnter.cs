using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyBox;
using ProjectBBF.Persistence;
using UnityEngine;

public class DialogueTimelineTestEnter : MonoBehaviour
{
    [SerializeField] private string _scene;
    [SerializeField] private string _fadeinDirectorKey;
    [SerializeField] private string _fadeoutDirectorKey;
    [SerializeField] private Vector2 _rollbackPos;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var loaderInst = SceneLoader.Instance;
            var persistenceInst = PersistenceManager.Instance;
            var blackboard = persistenceInst.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");
            
            blackboard.CurrentWorld = loaderInst.CurrentWorldScene;
            blackboard.CurrentPosition = _rollbackPos;
            
            GameObjectStorage.Instance.StoredObjects.ForEach(x =>
            {
                if (x.CompareTag("Player") && x.TryGetComponent(out PlayerController pc))
                {
                    pc.StateHandler.TranslateState("ToCutScene");
                }
            });
            
            TimeManager.Instance.Pause();

            _ = loaderInst
                    .WorkDirectorAsync(false, _fadeoutDirectorKey)
                    .ContinueWith(_ => SceneLoader.Instance.LoadWorldAsync(_scene))
                    .ContinueWith(_ => SceneLoader.Instance.WorkDirectorAsync(true, _fadeoutDirectorKey))
                ;
        }
    }
}