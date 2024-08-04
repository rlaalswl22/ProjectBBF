using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using MyBox;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerStateTranslator : MonoBehaviour, IPlayerStrategy
{
    public EPlayerControlState PeekState { get; set; }

    public void Init(PlayerController controller)
    {
    }

    public EPlayerControlState Pop()
    {
        var state = PeekState;
        PeekState = EPlayerControlState.None;
        return state;
    }

    public void OnUpdate()
    {
        // builder mode
        if (InputManager.Actions.BuilderMode.triggered)
        {
            if (PeekState != EPlayerControlState.Builder)
            {
                PeekState = EPlayerControlState.Builder;
            }
            else
            {
                PeekState = EPlayerControlState.Normal;
            }
        }
        
        // collecting
        if (InputManager.Actions.Collect.triggered)
        {
            PeekState = EPlayerControlState.Collection;
        }
    }
}

public enum EInputType
{
    Trigger,
    Pressed,
}

[UnitCategory("ProjectBBF/InputAction")]
public class InputActionUnit : Unit
{
    private ControlInput _input;
    private ControlOutput _outputSuccess;

    private ValueInput _action;
    private ValueInput _type;
    
    protected override void Definition()
    {
        _input = ControlInput("", Execute);
        _outputSuccess = ControlOutput("Key Input");

        _action = ValueInput<string>("Input Action");
        _type = ValueInput<EInputType>("Input Type");
    }

    private ControlOutput Execute(Flow flow)
    {
        var actionStr = flow.GetValue<string>(_action);

        var action= InputManager.Actions.Get().FindAction(actionStr);
        Debug.Assert(action != null, $"'{action}' input action을 찾을 수 없습니다.");
        
        if (action == null)
        {
            return null;
        }

        var type = flow.GetValue<EInputType>(_type);

        switch (type)
        {
            case EInputType.Trigger:
                return action.triggered ? _outputSuccess : null; 
            case EInputType.Pressed:
                return action.IsPressed() ? _outputSuccess : null;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }
}

public class PlayerStateTranslatorUnit : Unit
{
    private ControlInput _input;
    private ControlOutput _outputSuccess;
    private ControlOutput _outputFailure;

    private ValueInput _state;
    private ValueInput _playerController;
    
    
    protected override void Definition()
    {
        _input = ControlInput("", Update);
        _outputSuccess = ControlOutput("Success");
        _outputFailure = ControlOutput("Failure");

        _state = ValueInput<EPlayerControlState>("State");
        _playerController = ValueInput<PlayerController>("PlayerController");
    }

    private ControlOutput Update(Flow flow)
    {
        var controller = flow.GetValue<PlayerController>(_playerController);
        var translator = controller.Translator;

        if (flow.GetValue<EPlayerControlState>(_state) == translator.PeekState)
        {
            return _outputSuccess;
        }

        return _outputFailure;
    }
}