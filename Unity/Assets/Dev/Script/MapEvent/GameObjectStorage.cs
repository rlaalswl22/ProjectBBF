using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Singleton;
using UnityEngine;

[Singleton(ESingletonType.Global, 100)]
public class GameObjectStorage : MonoBehaviourSingleton<GameObjectStorage>
{
    private List<GameObject> _list = new(10);

    public IReadOnlyList<GameObject> List => _list;
    
    public override void PostInitialize()
    {
    }

    public override void PostRelease()
    {
    }

    public void AddGameObject(GameObject obj)
    {
        if (_list.Contains(obj)) return;
        _list.Add(obj);
    }

    public void RemoveGameObject(GameObject obj)
    {
        _list.Remove(obj);
    }
}
