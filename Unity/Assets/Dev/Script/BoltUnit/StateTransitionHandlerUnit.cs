using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("ProjectBBF")]
public class StateTransitionHandlerUnit : Unit
{
    [DoNotSerialize] private ControlInput _cOnUpdate;
    [DoNotSerialize] private ControlOutput _cBranchState;
    [DoNotSerialize] private ValueInput _vTargetStateKey;
    [DoNotSerialize] private ValueInput _vStateTransitionHandler;

    private string _targetStateKey = string.Empty;
    private StateTransitionHandler _stateTransitionHandler;
    
    protected override void Definition()
    {
        _cOnUpdate = ControlInput("Update", Update);
        _cBranchState = ControlOutput("Trigger");

        _vTargetStateKey = ValueInput<string>("TargetStateKey");
        _vStateTransitionHandler = ValueInput<StateTransitionHandler>("Handler");
    }

    private ControlOutput Update(Flow flow)
    {
        _targetStateKey = flow.GetValue<string>(_vTargetStateKey);
        _stateTransitionHandler = flow.GetValue<StateTransitionHandler>(_vStateTransitionHandler);

        return _stateTransitionHandler.CanTransfer(_targetStateKey) ? _cBranchState : null;
    }
}
