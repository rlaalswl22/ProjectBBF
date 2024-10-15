using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MoveToWorld : MonoBehaviour
{
    [SerializeField] private SceneName _scene;
    [SerializeField] private string _directorKey = "BlackAlpha";
    [SerializeField] private bool _fadeOut;
    [SerializeField] private bool _fadeIn;
    [SerializeField] private Transform _initPlayerPosition;

    public void MoveWorld()
    {
        var loaderInst = SceneLoader.Instance;

        var obj = GameObjectStorage.Instance.StoredObjects.FirstOrDefault(x => x.CompareTag("Player"));
        if (obj == false) return;
        if (obj.TryGetComponent(out PlayerController pc) is false) return;
        if (_initPlayerPosition == false) return;
        
        var pos = _initPlayerPosition.position;
        pc.Blackboard.IsMoveStopped = true;
        pc.Blackboard.IsInteractionStopped = true;
        if (_fadeOut)
        {
            _ = loaderInst
                    .WorkDirectorAsync(false, _directorKey)
                    .ContinueWith(_ => SceneLoader.Instance.LoadWorldAsync(_scene))
                    .ContinueWith(_ => pc.transform.position = pos)
                    .ContinueWith(async x =>
                    {
                        if (_fadeIn)
                        {
                            _ = await SceneLoader.Instance.WorkDirectorAsync(true, _directorKey);
                        }
                        
                        pc.Blackboard.IsMoveStopped = false;
                        pc.Blackboard.IsInteractionStopped = false;
                    })
                ; 
        }
        else
        {
            _ = loaderInst
                    .LoadWorldAsync(_scene)
                    .ContinueWith(_ => pc.transform.position = pos)
                    .ContinueWith(async x =>
                    {
                        if (_fadeIn)
                        {
                            _ = await SceneLoader.Instance.WorkDirectorAsync(true, _directorKey);
                        }
                        
                        pc.Blackboard.IsMoveStopped = false;
                        pc.Blackboard.IsInteractionStopped = false;
                    })
                ; 
        }
    }
}
