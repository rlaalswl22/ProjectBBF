﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DS.Runtime;
using ProjectBBF.Persistence;
using UnityEngine;
using DialogueBranchField = DS.Runtime.DialogueBranchField;

public class FrogRaceMinigameController : MinigameBase<FrogRaceMinigameData>
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private List<Transform> _tracks;

    private List<FrogRaceFrogObject> _frogs;

    private int _goalIndex;
    private int _targetIndex;
    private int _money;

    private CinemachineBrain _brain;

    protected override void OnGameInit()
    {
        _frogs ??= new List<FrogRaceFrogObject>(Data.Frogs.Length);
        _goalIndex = 0;
        _targetIndex = 0;
        _money = 0;

        using var enumerator = _tracks.GetEnumerator();
        foreach (var data in Data.Frogs)
        {
            if (enumerator.MoveNext() is false)
            {
                Debug.LogError("트랙이 모자람");
                return;
            }

            var obj = Instantiate(data.FrogObject, transform);
            obj.FrogData = data;
            obj.gameObject.SetActive(true);
            obj.transform.position = (Vector2)enumerator.Current!.position;
            _frogs.Add(obj);
        }

        _camera.gameObject.SetActive(true);
        _camera.MoveToTopOfPrioritySubqueue();
        _brain = Camera.main.GetComponent<CinemachineBrain>();
        _brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
    }

    protected override async UniTask OnTutorial()
    {
        Player.MoveStrategy.ResetVelocity();
        DialogueController.Instance.ResetDialogue();
        Player.StateHandler.TranslateState("ToDialogue");
        var inst = DialogueController.Instance;

        var fields = _frogs.Select(x => inst.GetField<BranchFieldButton>().Init(x.FrogData.DisplayName)).ToArray();
        var result = await inst.GetBranchResultAsync(
            fields, this.GetCancellationTokenOnDestroy()
        );

        _targetIndex = result.index;

        BranchFieldIntInput moneyInput;
        result = await inst.GetBranchResultAsync(
            new DialogueBranchField[]
            {
                moneyInput = inst.GetField<BranchFieldIntInput>().Init(100, 100, 100, 1000),
                inst.GetField<BranchFieldButton>().Init("경기 시작하기"),
                inst.GetField<BranchFieldButton>().Init("그만두기")
            }, this.GetCancellationTokenOnDestroy()
        );

        _money = moneyInput.GetResult().Value;

        if (result.index == 2)
        {
            RequestEndGame();
        }

        DialogueController.Instance.ResetDialogue();
        Player.StateHandler.TranslateState("EndOfDialogue");
    }

    protected override void OnGameBegin()
    {
        Player.MoveStrategy.IsStopped = true;
        StartCoroutine(CoUpdate());

        for (int i = 0; i < _frogs.Count; i++)
        {
            var frog = _frogs[i];
            frog.Begin(Data, Data.Frogs[i]);
        }
    }

    private IEnumerator CoUpdate()
    {
        while (true)
        {
            yield return null;
        }
    }

    protected override void OnGameRelease()
    {
        foreach (var frog in _frogs)
        {
            frog.End();
            frog.gameObject.SetActive(false);
            Destroy(frog.gameObject);
        }

        _frogs.Clear();
    }

    protected override void OnPreGameEnd(bool isRequestEnd)
    {
        _camera.gameObject.SetActive(false);
    }

    protected override bool IsGameEnd()
    {
        for (int i = 0; i < _frogs.Count; i++)
        {
            var frog = _frogs[i];
            if (frog.IsGoal)
            {
                _goalIndex = i;
                return true;
            }
        }

        return false;
    }

    protected override async UniTask OnGameEnd(bool isRequestEnd)
    {
        Player.MoveStrategy.IsStopped = false;
        if (isRequestEnd)
        {
            OnGameRelease();
            return;
        }

        Player.MoveStrategy.ResetVelocity();
        var blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");
        var inst = DialogueController.Instance;
        inst.ResetDialogue();
        Player.StateHandler.TranslateState("ToDialogue");
        inst.Visible = true;


        if (_goalIndex == _targetIndex)
        {
            var frogData = _frogs[_targetIndex];
            int money = (int)(frogData.FrogData.DividendRate * _money);
            blackboard.Money += money;
            inst.DialogueText = $"<color=#826209>{money}</color> 원 벌었습니다.";
        }
        else
        {
            inst.DialogueText = $"돈을 잃었습니다.";
        }

        OnGameRelease();
        await UniTask.WaitUntil(() => InputManager.Map.UI.DialogueSkip.triggered, PlayerLoopTiming.Update);

        inst.ResetDialogue();
    }
}