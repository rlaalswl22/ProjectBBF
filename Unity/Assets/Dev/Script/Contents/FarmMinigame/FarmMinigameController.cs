using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DS.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FarmMinigameController : MinigameBase
{
    [SerializeField] private FarmlandManager _farmlandManager;
    
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;

    [SerializeField] private Light2D _light;
    [SerializeField] private Transform _lightOnPoint;
    [SerializeField] private Transform _lightOffPoint;

    private int _currentItemCount = 0;

    private PlayerController _pc;
    
    protected override async UniTask OnSignalAsync()
    {
        foreach (var obj in GameObjectStorage.Instance.StoredObjects)
        {
            if (obj.CompareTag("Player") && obj.TryGetComponent(out PlayerController pc))
            {
                var data = Data as FarmMinigameData;
                _pc = pc;
                DialogueController.Instance.ResetDialogue();
                await SceneLoader.Instance.WorkDirectorAsync(false, Data.DirectorKey);
                
                pc.transform.position = _startPoint.position;
                _currentItemCount = 0;

                pc.Inventory.Model.OnPushItem += OnItemCount;
                
                await SceneLoader.Instance.WorkDirectorAsync(true, Data.DirectorKey);
                
                
                _pc.StateHandler.TranslateState("ToDialogue");
                await _pc.Dialogue.RunDialogue(data.DialogueTutorial).ContinueWith(_ =>
                {
                    _pc.StateHandler.TranslateState("EndOfDialogue");
                });
                
                StartCoroutine(CoUpdate(pc));
                StartCoroutine(OnObserveItemCount(pc));
                
                break;
            }
        }
    }

    protected override async UniTask OnEndSignalAsync()
    {
        if (_pc)
        {
            var data = Data as FarmMinigameData;
            
            await SceneLoader.Instance.WorkDirectorAsync(false, Data.DirectorKey).ToCoroutine();
            ResetGame(_pc);
            await  SceneLoader.Instance.WorkDirectorAsync(true, Data.DirectorKey).ToCoroutine();
            
            _pc.StateHandler.TranslateState("ToDialogue");
            _ = _pc.Dialogue.RunDialogue(data.DialogueAfterGameExit).ContinueWith(_ =>
            {
                _pc.StateHandler.TranslateState("EndOfDialogue");
            });
        }
    }

    private void OnItemCount(ItemData itemData, int count, GridInventoryModel model)
    {
        var gameData = Data as FarmMinigameData;
        
        if (itemData == false) return;
        if (itemData == gameData.GoalItem)
        {
            _currentItemCount += count;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _light.gameObject.SetActive(false);
    }

    private IEnumerator OnObserveItemCount(PlayerController pc)
    {
        var data = Data as FarmMinigameData;
        while (_currentItemCount < data.GoalItemCount)
        {
            yield return null;
        }
        
        
        pc.Inventory.Model.OnPushItem -= OnItemCount;
        
        yield return SceneLoader.Instance.WorkDirectorAsync(false, Data.DirectorKey).ToCoroutine();
        ResetGame(pc);
        yield return SceneLoader.Instance.WorkDirectorAsync(true, Data.DirectorKey).ToCoroutine();

        
        pc.StateHandler.TranslateState("ToDialogue");
        _ = pc.Dialogue.RunDialogue(data.DialogueAfterGameEnd).ContinueWith(_ =>
        {
            pc.StateHandler.TranslateState("EndOfDialogue");
        });
        StopAllCoroutines();
        Release();
    }

    private void ResetGame(PlayerController pc)
    {
        pc.transform.position = _endPoint.position;
        _light.gameObject.SetActive(false);
        DOTween.Kill(this);
        _farmlandManager.ResetFarm();
        DialogueController.Instance.ResetDialogue();
    }

    private IEnumerator CoUpdate(PlayerController pc)
    {
        var data = Data as FarmMinigameData;
        
        Debug.Assert(data is not null);
        
        _light.gameObject.SetActive(true);
        _light.intensity = data.LightOffIntensity;
        _light.transform.position = (Vector2)_lightOffPoint.position;

        while (true)
        {
            yield return new WaitForSeconds(data.LightIgnitionBeginWaveTime);
            
            Sequence sequence = DOTween.Sequence();
            sequence.Join
            (
                _light.transform.DOMove((Vector2)_lightOnPoint.position, data.LightOnMoveSpeed).SetEase(Ease.Linear)
            );
            sequence.Join
            (
                DOTween.To(()=>_light.intensity, x => _light.intensity = x, data.LightOnIntensity, 0.25f)
            );
            sequence.SetId(this);
            
            yield return new WaitForSeconds(data.LightIgnitionEndWaveTime);
            
            _farmlandManager.GrowUp(1);
            
            sequence = DOTween.Sequence();
            sequence.Join
            (
                _light.transform.DOMove((Vector2)_lightOffPoint.position, data.LightOffMoveSpeed).SetEase(Ease.Linear)
            );
            sequence.Join
            (
                DOTween.To(()=>_light.intensity, x => _light.intensity = x, data.LightOffIntensity, 0.25f)
            );
            sequence.SetId(this);
            
            yield return sequence;
        }
    }
}
