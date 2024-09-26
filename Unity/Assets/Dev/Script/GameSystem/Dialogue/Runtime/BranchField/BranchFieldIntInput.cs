using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DS.Runtime;
using TMPro;
using UnityEngine;

public class BranchResultIntInput : DialogueBranchResult
{
    public int Value;

    public BranchResultIntInput(int value)
    {
        Value = value;
    }
}

public class BranchFieldIntInput : DialogueBranchField, IValuable<BranchResultIntInput>
{
    [SerializeField] private TMP_InputField _inputField;

    private BranchResultIntInput _result;

    private int _step = 1;
    private int _min;
    private int _max;
    
    private void Awake()
    {
        _inputField.onValueChanged.AddListener(OnInput);
    }

    public BranchFieldIntInput Init(int initalValue, int step, int min, int max)
    {
        _result = new BranchResultIntInput(initalValue);
        _step = step;
        _min = min;
        _max = max;
        
        return this;
    }

    void OnInput(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            if (!int.TryParse(input, out int value))
            {
                _inputField.text = _result.Value.ToString();
            }
            else
            {
                _result.Value = Mathf.Clamp(value, _min, _max);
            }
        }
    }

    public void Add(int dir)
    {
        _result.Value = Mathf.Clamp(_result.Value + (dir > 0 ? 1 : -1) * _step, _min, _max);
        
        _inputField.SetTextWithoutNotify(_result.Value.ToString());
    }

    public override void DestroySelf()
    {
        Destroy(gameObject);
    }

    public BranchResultIntInput GetResult()
    {
        return _result;
    }
}
