using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void GotoGameWorld()
    {
        SceneLoader.Instance.LoadWorldAsync("World_DaffodilLake_Arakar")
            .ContinueWith(_ => SceneLoader.Instance.LoadImmutableScenesAsync())
            .Forget();
    }
}
