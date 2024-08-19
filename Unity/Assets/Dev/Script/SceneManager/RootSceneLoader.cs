using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using MyBox;

/// <summary>
/// Allows setting a scene as a root scene and setting its child scenes. To use this, drag this component on any object in a scene to make that scene a root scene. In the background, ChildSceneLoader will automatically manage this.
/// </summary>
public class RootSceneLoader : MonoBehaviour
{
    [SerializeField] private bool _loadImmutableScene = true;
    [field: SerializeField, HideInInspector] private List<string> _scenes;

    public async UniTask<List<(string, AsyncOperation)>> LoadAsync()
    {
        if (_scenes == null) return new List<(string, AsyncOperation)>();
        
        List<UniTask> tasks = new List<UniTask>(_scenes.Count);
        List<(string, AsyncOperation)> loadedScenes = new List<(string, AsyncOperation)>(_scenes.Count);

        foreach (string sceneName in _scenes)
        {
            if (SceneManager.GetActiveScene().name == sceneName) continue;
            if (SceneLoader.Instance.ImmutableSceneTable.Scenes.Contains(sceneName)) continue;
            
            AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            oper.allowSceneActivation = false;

            string ls = sceneName;

            var task = UniTask
                .WaitUntil(() => oper.progress >= 0.9f, PlayerLoopTiming.Update, GlobalCancelation.PlayMode)
                .ContinueWith(()=>loadedScenes.Add((ls, oper)));
            
            tasks.Add(task);
        }

        await UniTask.WhenAll(tasks);

        return loadedScenes;
    }

#if UNITY_EDITOR
    [SerializeField] public List<SceneAsset> ChildScenesToLoadConfig;

    private void Start()
    {
        if (SceneLoader.Instance.IsLoadedImmutableScenes == false)
        {
            string worldSceneName = gameObject.scene.name;
            _ = SceneLoader.Instance
                .LoadWorldAsync(worldSceneName, true)
                .ContinueWith(x=>
                {
                    if (x && _loadImmutableScene)
                    {
                        _ = SceneLoader.Instance.LoadImmutableScenesAsync().ContinueWith(() =>
                        {
                            SceneManager.GetSceneByName(worldSceneName).GetRootGameObjects().ForEach(y =>
                            {
                                var root = y.GetComponent<RootSceneLoader>();

                                if (root == false)
                                {
                                    root = y.GetComponentInChildren<RootSceneLoader>();
                                }
            
                                if (root)
                                {
                                    var t = GameObjectStorage.Instance.List.FirstOrDefault(z =>
                                        z.GetComponent<PlayerController>());
                                    
                                    if(t)
                                        t.transform.SetXY(root.transform.position);
                                }
                            });
                        });
                    }
                });
        }
    }

    private void Update()
    {
        // DO NOT DELETE keep this so we can enable/disable this script... (used in ChildSceneLoader)
    }
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        if (ChildScenesToLoadConfig is null) return;
        
        _scenes = new List<string>();
        
        ChildScenesToLoadConfig.ForEach(x=>
        {
            if (EditorSceneManager.GetActiveScene().name == x.name)
            {
                return;
            }
            
            _scenes.Add(x.name);
        });

    }

    public void ResetSceneSetupToConfig()
    {
        var sceneAssetsToLoad = ChildScenesToLoadConfig;
        
        if (ChildScenesToLoadConfig.FirstOrDefault(x => x.name == EditorSceneManager.GetActiveScene().name) is null)
        {
            return;
        }

        List<SceneSetup> sceneSetupToLoad = new List<SceneSetup>();
        foreach (var sceneAsset in sceneAssetsToLoad)
        {
            sceneSetupToLoad.Add(new SceneSetup()
                { path = AssetDatabase.GetAssetPath(sceneAsset), isActive = false, isLoaded = true });
        }

        sceneSetupToLoad[0].isActive = true;
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.RestoreSceneManagerSetup(sceneSetupToLoad.ToArray());
    }
#endif
}

#if UNITY_EDITOR


[InitializeOnLoad]
public class ChildSceneLoader
{
    static ChildSceneLoader()
    {
        EditorSceneManager.sceneOpened += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene _, OpenSceneMode mode)
    {
        if (mode != OpenSceneMode.Single || BuildPipeline.isBuildingPlayer)
            return; // try to load child scenes only for root scenes or if not building

        var scenesToLoadObjects = GameObject.FindObjectsOfType<RootSceneLoader>();
        if (scenesToLoadObjects.Length > 1)
        {
            throw new Exception("Should only have one root scene at once loaded");
        }

        if (scenesToLoadObjects.Length == 0 ||
            !scenesToLoadObjects[0].enabled) // only when we have a config and when that config is enabled
        {
            return;
        }

        scenesToLoadObjects[0].ResetSceneSetupToConfig();

        Debug.Log("Setup done for root scene and child scenes");
    }
}
#endif