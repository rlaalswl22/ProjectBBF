using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectBBF.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

[Singleton(ESingletonType.Global, 1)]
public class MapPortalManager : MonoBehaviourSingleton<MapPortalManager>
{
    private Dictionary<string, Vector2> _table = new();
    public override void PostInitialize()
    {
        SceneLoader.Instance.WorldLoaded += OnLoaded;
    }

    public override void PostRelease()
    {
    }

    public void TryAdd(string key, Vector2 pos)
    {
        _table.TryAdd(key, pos);
    }
    
    private void OnLoaded(string s)
    {
        if (_playerTransform && _table.TryGetValue(_targetPortalKey, out var targetPortalPosition))
        {
            var pos = new Vector3(
                targetPortalPosition.x,
                targetPortalPosition.y,
                _playerTransform.position.z
            );

            _playerTransform.position = pos;
        }
    }

    
    private string _targetPortalKey;
    private Transform _playerTransform;
    
    public UniTask Move(string worldSceneName, string targetPortalKey, Transform playerTransform)
    {
        var inst = SceneLoader.Instance;
        if(inst.IsProgress) return new UniTask<bool>(false);

        _targetPortalKey = targetPortalKey;
        _playerTransform = playerTransform;
        return inst.LoadWorldAsync(worldSceneName);
    }
}
