﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using ProjectBBF.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class MoleMinigameController : MinigameBase<MoleMinigameData>
{
    [Serializable]
    public class Pivot
    {
        public Transform Transform;

        [NonSerialized] public bool Used;
    }

    [SerializeField] private CinemachineVirtualCamera _camera;

    [SerializeField] private GameTimerUI _timerUI;
    [SerializeField] private GameScoreUI _scoreUI;
    [SerializeField] private GameObject _uiPanel;

    [SerializeField] private List<Pivot> _pivots;
    private Dictionary<int, Stack<MoleGameObject>> _pool = new();
    private Dictionary<int, MoleMinigameData.Mole> _moleTable = new();
    private List<MoleGameObject> _currentMoles = new();
    private CancellationTokenSource _gameCts;
    private CinemachineBrain _brain;
    
    private float _gameTimer;
    private MoleMinigameData.Reward _lastReward;
    private int _currentScore;

    private int Score
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            _scoreUI.Score = value;
        }
    }

    private float GameTime
    {
        get => _gameTimer;
        set
        {
            _gameTimer = value;
            _timerUI.Time =  new GameTime((int)Data.GameDuration - (int)_gameTimer);
        }
    }

    private MoleMinigameData.Stage? GetCurrentStage()
    {
        foreach (MoleMinigameData.Stage stage in Data.Stages)
        {
            if (stage.MaxStageTime >= _gameTimer)
            {
                return stage;
            }
        }

        return null;
    }

    private MoleGameObject GetMoleObjectFromPool(MoleMinigameData.Stage stage)
    {
        float currentRate = Random.value;
        float rateSum = 0f;

        int key = -1;

        foreach (int moleKey in stage.MoleKeyList)
        {
            MoleMinigameData.Mole data = _moleTable[moleKey];
            rateSum += data.AppearRate;

            if (rateSum >= currentRate)
            {
                key = moleKey;
                break;
            }
        }

        if (key == -1)
        {
            key = stage.MoleKeyList[^1];
        }

        MoleMinigameData.Mole moleData = _moleTable[key];
        MoleGameObject obj = null;
        if (_pool[key].Any() is false)
        {
            obj = moleData.Prefab.Clone();
            obj.Init(Data, moleData);
        }
        else
        {
            obj = _pool[key].Pop();
        }

        obj.ResetObj();
        _currentMoles.Add(obj);
        return obj;
    }

    private Pivot GetRandomPivot()
    {
        var list = _pivots.Where(x => x.Used is false).ToList();
        if (list.Count == 0) return null;

        int rand = Random.Range(0, list.Count - 1);
        Pivot pivot = list[rand];
        return pivot;
    }

    private void ClearCurrentMole()
    {
        foreach (MoleGameObject mole in _currentMoles)
        {
            int index = mole.MoleData.Key;

            mole.gameObject.SetActive(false);
            _pool[index].Push(mole);
        }

        _currentMoles.Clear();
    }

    private void ReturnMole(MoleGameObject obj)
    {
        _currentMoles.Remove(obj);

        int index = obj.MoleData.Key;

        _pool[index].Push(obj);
    }

    protected override void Awake()
    {
        base.Awake();

        _uiPanel.SetActive(false);
        _timerUI.Visible = false;
        _scoreUI.Visible = false;
        _camera.gameObject.SetActive(false);
    }

    protected override void OnGameInit()
    {
        _lastReward = default;
        _timerUI.Visible = true;
        _scoreUI.Visible = true;
        
        _camera.gameObject.SetActive(true);
        _camera.MoveToTopOfPrioritySubqueue();
        _brain = Camera.main.GetComponent<CinemachineBrain>();
        _brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        
        Player.HudController.Visible = false;

        _gameCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
        Score = 0;
        GameTime = 0;

        _uiPanel.SetActive(true);
        foreach (MoleMinigameData.Mole mole in Data.Moles)
        {
            int moleCloneCount = Data
                .Stages
                .Where(x => x.MoleKeyList.Contains(mole.Key))
                .Max(x => x.AppearMaxCount);

            var poolStack = new Stack<MoleGameObject>(3);
            for (int i = 0; i < moleCloneCount; i++)
            {
                MoleGameObject obj = mole.Prefab.Clone();
                obj.Init(Data, mole);
                obj.ResetObj();
                poolStack.Push(obj);
            }

            int index = mole.Key;
            _moleTable.Add(index, mole);
            _pool.Add(index, poolStack);
        }

        foreach (var pivot in _pivots)
        {
            pivot.Used = false;
        }
    }

    protected override async UniTask OnTutorial()
    {
        await RunDialogue(Data.Tutorial);
    }

    protected override void OnGameBegin()
    {
        Player.HudController.Visible = false;
        StartCoroutine(CoUpdate());
        StartCoroutine(CoTimer());
    }

    private IEnumerator CoTimer()
    {
        while (true)
        {
            GameTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator CoUpdate()
    {
        while (true)
        {
            MoleMinigameData.Stage? stage = GetCurrentStage();

            if (stage is null)
            {
                yield return null;
                continue;
            }

            int iter = stage.Value.AppearMaxCount - _currentMoles.Count;
            for (int i = 0; i < iter; i++)
            {
                MoleGameObject moleObj = GetMoleObjectFromPool(stage.Value);

                if (moleObj == false) continue;
                _ = UpdateMole(moleObj);
            }

            yield return new WaitForSeconds(stage.Value.AppearInterval);
        }
    }

    private async UniTask UpdateMole(MoleGameObject moleObj)
    {
        if (_gameCts is null || _gameCts.IsCancellationRequested) return;

        Pivot pivot = GetRandomPivot();

        if (pivot is null)
        {
            Debug.LogError("남는 pivot이 없음");
            return;
        }

        try
        {
            pivot.Used = true;
            moleObj.ResetObj();
            moleObj.transform.position = (Vector2)pivot.Transform.position;

            bool canceled = await moleObj.WaitAppearAsync(_gameCts?.Token ?? default).SuppressCancellationThrow();

            if (moleObj.IsHit)
            {
                Score += moleObj.MoleData.AcquisitionScore;
            }

            if (canceled) return;

            canceled = await moleObj.WaitDisappearAsync(_gameCts?.Token ?? default).SuppressCancellationThrow();

            if (moleObj == false)
            {
                pivot.Used = false;
                return;
            }

            ReturnMole(moleObj);
            moleObj.ResetObj();
            pivot.Used = false;
        }
        catch (Exception e)when (e is not OperationCanceledException)
        {
            Debug.LogException(e);
        }
    }

    protected override void OnPreGameEnd(bool isRequestEnd)
    {
        
        Player.HudController.Visible = true;
        _timerUI.Visible = false;
        _scoreUI.Visible = false;
        _uiPanel.SetActive(false);
        _camera.gameObject.SetActive(false);
        OnGameRelease();

        if (isRequestEnd) return;

        var blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");
        var reward = Data
            .Rewards
            .Where(x => x.TargetScore <= Score)
            .OrderByDescending(x => x.TargetScore)
            .FirstOrDefault();

        if (reward.Item)
        {
            blackboard.Inventory.Model.PushItem(reward.Item, reward.Count);
            _lastReward = reward;
        }
    }

    protected override void OnGameRelease()
    {
        _gameCts?.Cancel();
        _gameCts = null;
        Player.HudController.Visible = true;


        _camera.gameObject.SetActive(false);

        ClearCurrentMole();

        foreach (var stack in _pool.Values)
        {
            while (stack.Any())
            {
                var mole = stack.Pop();
                mole.DestroySelf();
            }
        }

        _moleTable.Clear();
        _pool.Clear();

        StopAllCoroutines();
    }

    protected override bool IsGameEnd()
    {
        return Data.GameDuration < _gameTimer;
    }

    protected override async UniTask OnGameEnd(bool isRequestEnd)
    {
        if (isRequestEnd)
        {
            return;
        }

        if (_lastReward.Item == false)
        {
            return;
        }

        string str = $"축하합니다! {_lastReward.Rank}등입니다! 상품은 {_lastReward.Item.ItemName}입니다!";
        
        var inst = DialogueController.Instance;
        inst.Visible = true;
        inst.DialogueText = str;


        await UniTask.WaitUntil(() => InputManager.Map.UI.DialogueSkip.triggered, PlayerLoopTiming.Update,
            this.GetCancellationTokenOnDestroy());
                
        inst.ResetDialogue();

        return;
    }
}