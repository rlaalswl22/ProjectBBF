using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using ProjectBBF.Persistence;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class PatrollPointPathFinder : StateBehaviour
{
    private int? _currentIndex;
    private PatrolPointPath _before;

    public bool IsInit { get; private set; }

    private bool _loadFlag;

    private ActorPersistenceObject _persistenceObject;

    public void TryLoad(string saveKey)
    {
        if (IsInit) return;
        IsInit = true;
        
        if (string.IsNullOrEmpty(saveKey)) return;
        _persistenceObject = PersistenceManager.Instance.LoadOrCreate<ActorPersistenceObject>(saveKey);
        if (_persistenceObject.PatrolPointIndex != 0)
        {
            _loadFlag = true;
            _currentIndex = _persistenceObject.PatrolPointIndex;
        }
    }


    public void Reduce()
    {
        if (_currentIndex.HasValue)
        {
            _currentIndex = Mathf.Max(_currentIndex.Value - 1, 0);
        }
    }


    private void OnDestroy()
    {
        if (_currentIndex is null) return;
        if (PersistenceManager.Instance == false) return;

        if (_persistenceObject is not null)
        {
            _persistenceObject.PatrolPointIndex = _currentIndex.Value;
        }
    }

    [CanBeNull]
    public PatrolPoint GetNextPoint(PatrolPointPath path)
    {
        if (path is null) return null;
        if (_loadFlag)
        {
            _loadFlag = false;

            if (_currentIndex.HasValue && path.PatrollPoints.Count > _currentIndex.Value)
            {
                return path.PatrollPoints[_currentIndex.Value];
            }
        }
        
        if (_before != path)
        {
            if (_before != null)
            {
                _currentIndex = null;
            }
            _before = path;
        }

        if (_currentIndex == null)
        {
            if (path.PatrollPoints.Any())
            {
                _currentIndex = 0;
                return path.PatrollPoints[_currentIndex.Value];
            }

            return null;
        }
        else
        {
            _currentIndex++;
            if (path.PatrollPoints.Count <= _currentIndex)
            {
                _currentIndex = path.Loop is false ? path.PatrollPoints.Count - 1 : 0;
            }

            return path.PatrollPoints[_currentIndex.Value];
        }
    }
}

[UnitCategory("ProjectBBF")]
[UnitTitle("PatrollPointPathFinder")]
public class PatrolPointPathFinderUnit : Unit
{
    private ValueInput _vPatrollPointPath;
    private ValueInput _vSaveKey;
    private ValueOutput _vPoint;

    private ControlInput _cStart;
    private ControlInput _cReduce;
    private ControlOutput _cOutput;

    private PatrolPoint _lastPoint;

    private ControlOutput OnExecute(Flow flow)
    {
        var behaviour = BehaviourBinder.GetBinder(flow)
            .GetBehaviour<PatrollPointPathFinder>();

        behaviour.TryLoad(flow.GetValue<string>(_vSaveKey));
        _lastPoint = behaviour.GetNextPoint(
            flow.GetValue<PatrolPointPath>(_vPatrollPointPath)
        );


        if (_lastPoint is null)
        {
            return null;
        }

        return _cOutput;
    }

    private ControlOutput OnReduce(Flow flow)
    {
        BehaviourBinder.GetBinder(flow)
            .GetBehaviour<PatrollPointPathFinder>()
            .Reduce()
            ;

        return null;
    }

    protected override void Definition()
    {
        _vPatrollPointPath = ValueInput<PatrolPointPath>("Patrol point path");
        _vSaveKey = ValueInput<string>("actor save key");
        _vPoint = ValueOutput("Patrol point", x => _lastPoint);

        _cOutput = ControlOutput("");
        _cStart = ControlInput("OnEnterState", OnExecute);
        _cReduce = ControlInput("Reduce", OnReduce);
    }
}