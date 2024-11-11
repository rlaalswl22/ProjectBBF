using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectBBF.Persistence;
using UnityEngine;

public class PlayerGameQuitPresenter : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void GotoMainMenu()
    {
        PersistenceManager.Instance.SaveGameDataCurrentFileName();
        
        SceneLoader.Instance.WorkDirectorAsync(false, "BlackAlpha")
            .ContinueWith(_ => SceneLoader.Instance.UnloadImmutableScenesAsync())
            .ContinueWith(_ => SceneLoader.Instance.LoadWorldAsync("World_MainMenu"))
            .ContinueWith(_ => SceneLoader.Instance.WorkDirectorAsync(true, "BlackAlpha"))
            .Forget();
    }
}
