using System;
using System.Collections;
using ProjectBBF.Persistence;
using UnityEngine;

public class FirstWorldLoadedTrigger : MapTriggerBase
{
    [SerializeField] private string _saveKey;
    
    public const string PERSISTENCE_KEY = "WorldMapLoadedTrigger"; 
    
    private void Start()
    {
        StartCoroutine(CoUpdate());
    }

    private IEnumerator CoUpdate()
    {
        if (PersistenceManager.Instance == false) yield break;
        
        var obj = PersistenceManager.Instance.LoadOrCreate<WorldLoadedTriggerPersistenceObject>(PERSISTENCE_KEY);

        if (obj.Keys.Contains(_saveKey))
        {
            yield break;
        }
        
        while (true)
        {
            if (GameObjectStorage.Instance.TryGetPlayerController(out var pc))
            {
                Trigger(pc.Interaction);
                obj.Keys.Add(_saveKey);
                break;
            }
            else
            {
                yield return null;
            }
        }
    }
}