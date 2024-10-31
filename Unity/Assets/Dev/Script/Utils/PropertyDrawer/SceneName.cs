using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[System.Serializable]
public class SceneName
{
    #if UNITY_EDITOR
    [SerializeField] private SceneAsset _sceneAsset;
    #endif
    
    [SerializeField] private string _sceneName;

#if UNITY_EDITOR
    public string Scene
    {
        get
        {
            if (_sceneAsset == false)
            {
                return string.Empty;
            }
            
            _sceneName = _sceneAsset.name;

            return _sceneName;
        }
    }
#else
    public string Scene => _sceneName;
#endif
    
    public static implicit operator string(SceneName name)
    {
        if(name is null) return string.Empty;

        return name.Scene;
    }
}