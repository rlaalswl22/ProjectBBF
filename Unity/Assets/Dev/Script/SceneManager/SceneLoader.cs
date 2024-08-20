using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MyBox;
using ProjectBBF.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

[Singleton(ESingletonType.Global)]
public class SceneLoader : MonoBehaviourSingleton<SceneLoader>
{
    private List<string> _loadedAddtiveScenes;
    public ImmutableSceneTable ImmutableSceneTable { get; private set; }

    public bool IsLoadedImmutableScenes { get; private set; }
    public bool IsProgress { get; private set; }
    public IReadOnlyList<string> LoadedAddtiveScenes => _loadedAddtiveScenes;

    public event Action<string> WorldPreLoaded;
    public event Action<string> WorldLoaded;
    public event Action<string> WorldPostLoaded;

    public string DirectorKey { get; set; } = "BlackAlpha";

    private Dictionary<string, ScreenDirector> _directors;
    
    public string CurrentWorldScene { get; private set; }
    
    public override void PostInitialize()
    {
        ImmutableSceneTable = Resources.Load<ImmutableSceneTable>("Data/ImmutableSceneTable");
        var directorList = Resources.LoadAll<ScreenDirector>("Feature/ScreenDirector");
        
        print(directorList.Length);

        _directors = new();
        directorList.ForEach(x =>
        {
            var com = GameObject.Instantiate(x, transform);
            com.Enabled = false;
            _directors[x.Key] = com;
        });
        
        _loadedAddtiveScenes = new(5);
    }

    public override void PostRelease()
    {
        _loadedAddtiveScenes = null;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.M))
    //    {
    //        LoadWorldAsync("World_DaffodilLake_Arakar").ContinueWith(_ => LoadImmutableScenesAsync()).Forget();
    //    }
    //}

    private async UniTask UnloadAllAdditiveSceneAsync()
    {
        List<UniTask> tasks = new List<UniTask>(_loadedAddtiveScenes.Count);
        
        foreach (string sceneName in _loadedAddtiveScenes)
        {
            if (ImmutableSceneTable.Scenes.Contains(sceneName)) continue;
            
            AsyncOperation oper = SceneManager.UnloadSceneAsync(sceneName);
            if (oper is null) continue;
            
            oper.allowSceneActivation = true;

            var task = oper
                .ToUniTask(null, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
            
            tasks.Add(task);
        }
        
        await UniTask.WhenAll(tasks);
        _loadedAddtiveScenes.Clear();
    }

    public async UniTask LoadImmutableScenesAsync()
    {
        if (IsProgress) return;
        if (IsLoadedImmutableScenes) return;

        IsProgress = true;
        
        TryLoadEntryScene();
        
        List<UniTask> tasks = new List<UniTask>(ImmutableSceneTable.Scenes.Count);
        
        foreach (string sceneName in ImmutableSceneTable.Scenes)
        {
            AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (oper is null) continue;
            oper.allowSceneActivation = true;

            var task = oper
                .ToUniTask(null, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
            
            tasks.Add(task);
        }
        
        await UniTask.WhenAll(tasks);

        IsLoadedImmutableScenes = true;
        IsProgress = false;
    }

    public async UniTask UnloadImmutableScenesAsync()
    {
        if (IsProgress) return;
        if (IsLoadedImmutableScenes == false) return;

        IsProgress = true;
        
        List<UniTask> tasks = new List<UniTask>(ImmutableSceneTable.Scenes.Count);
        
        foreach (string sceneName in ImmutableSceneTable.Scenes)
        {
            AsyncOperation oper = SceneManager.UnloadSceneAsync(sceneName);
            if (oper is null) continue;

            var task = oper
                .ToUniTask(null, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
            
            tasks.Add(task);
        }
        
        await UniTask.WhenAll(tasks);

        IsLoadedImmutableScenes = false;
        IsProgress = false;
    }

    public async UniTask<bool> LoadWorldAsync(string worldSceneName, bool skipDirector = false)
    {
        if (IsProgress) return false;
        IsProgress = true;

        try
        {
            if (_directors.TryGetValue(DirectorKey, out var director) && skipDirector == false)
            {
                director.Enabled = true;
                await director.Fadeout();
            }
            
            if (ImmutableSceneTable.Scenes.Contains(worldSceneName))
            {
                Debug.LogError("Immutable scene은 single로 불러올 수 없습니다.");
                IsProgress = false;
                return false;
            }
            
            
            WorldPreLoaded?.Invoke(worldSceneName);

            TryLoadEntryScene();

            await UnloadAllAdditiveSceneAsync();

            var oper = SceneManager.LoadSceneAsync(worldSceneName, LoadSceneMode.Additive);

            if (oper is null)
            {
                IsProgress = false;
                Debug.LogError($"World scene({worldSceneName})을 불러오는데 실패했습니다.");
                return false;
            }
            
            oper.allowSceneActivation = true;
            await oper.ToUniTask(null, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);

            _loadedAddtiveScenes.Add(worldSceneName);

            var loaders = GameObject.FindObjectsOfType<RootSceneLoader>();
            if (loaders.Length > 1)
            {
                Debug.LogError("Loader는 Root이어야 합니다.");
                IsProgress = false;
                return false;
            }

            if (loaders.Length < 1)
            {
                IsProgress = false;
                return false;
            }

            var loader = loaders[0];
            List<(string, AsyncOperation)> loadedScenes = await loader.LoadAsync();

            _loadedAddtiveScenes.AddRange(loadedScenes.Select(x=>x.Item1));

            loadedScenes.ForEach(x => x.Item2.allowSceneActivation = true);

            await UniTask.WhenAll(loadedScenes.Select(x =>
                x.Item2.ToUniTask(null, PlayerLoopTiming.Update, GlobalCancelation.PlayMode)));

            _ = Resources.UnloadUnusedAssets().WithCancellation(GlobalCancelation.PlayMode);

            WorldLoaded?.Invoke(worldSceneName);
            await UniTask.Yield();
            WorldPostLoaded?.Invoke(worldSceneName);
        
            if (skipDirector == false && director)
            {
                await director.Fadein();
                director.Enabled = false;
            }

        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            Debug.LogException(e);
        }
        finally
        {
            IsProgress = false;
        }

        CurrentWorldScene = worldSceneName;

        return true;
    }

    private void TryLoadEntryScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        if (currentSceneName == "EntryScene") return;
        SceneManager.LoadScene("EntryScene", LoadSceneMode.Single);
    }
}
