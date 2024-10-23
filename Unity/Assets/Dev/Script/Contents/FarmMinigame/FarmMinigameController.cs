using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DS.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FarmMinigameController : MinigameBase<FarmMinigameData>
{
    [SerializeField] private FarmlandManager _farmlandManager;
    [SerializeField] private GameObject _particle;
    
    private int _currentItemCount = 0;

    protected override void Awake()
    {
        base.Awake();

        SetPlayParticle(false);
    }

    private void SetPlayParticle(bool value)
    {
        var arr = _particle.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem particle in arr)
        {
            if (value)
            {
                particle.Play();
            }
            else
            {
                particle.Stop();
            }
        }
        
        _particle.SetActive(value);
    }

    private IEnumerator CoUpdate()
    {
        Debug.Assert(Data is not null);
        
        while (true)
        {
            yield return new WaitForSeconds(Data.GrownInterval);
            _farmlandManager.GrowUpWithoutWetReset(1);
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
    protected override void OnGameInit()
    {
        var data = Data as FarmMinigameData;

        SetPlayParticle(true);
        Player.Inventory.Model.OnPushItem += OnItemCount;
    }

    protected override async UniTask OnTutorial()
    {
        var data = Data as FarmMinigameData;
        await RunDialogue(data.DialogueTutorial);
    }

    protected override void OnGameBegin()
    {
        StartCoroutine(CoUpdate());
    }

    protected override void OnGameRelease()
    {
        _farmlandManager.ResetFarm();
        DialogueController.Instance.ResetDialogue();
        Player.Inventory.Model.OnPushItem -= OnItemCount;
        _currentItemCount = 0;
        SetPlayParticle(false);
        
        StopAllCoroutines();
    }

    protected override bool IsGameEnd()
    {
        var data = Data as FarmMinigameData;

        return _currentItemCount >= data.GoalItemCount;
    }

    protected override void OnPreGameEnd(bool isRequestEnd)
    {
        base.OnPreGameEnd(isRequestEnd);
        
        SetPlayParticle(false);
    }

    protected override UniTask OnGameEnd(bool isRequestEnd)
    {
        return UniTask.CompletedTask;
    }
}
