using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DS.Core;
using DS.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialogueContext
{
    private DialogueRuntimeTree _tree;
    private float _duration;
    private Action<string> _textInput;

    //TODO: 향후 삭제 바람
    //private NpcController _controller;

    private DialogueView _view;

    private CancellationTokenSource _source;

    public bool CanNext => CurrentNode is not null;

    public DialogueRuntimeNode CurrentNode { get; private set; }
    
    public bool IsRunning { get; private set; }

    public async UniTask Next()
    {
        if (CanNext == false) return;
        if (IsRunning) return;

        _source?.Cancel();
        
        if (_source is null)
        {
            _source = CancellationTokenSource.CreateLinkedTokenSource(
                GlobalCancelation.PlayMode,
                new CancellationToken()
            );
        }

        DialogueItem item = null;
        IsRunning = true;
        try
        {
            begin:
        
            item = CurrentNode.CreateItem();
            
            if (item is TextItem textItem)
            {
                await TextUtil.DoTextUniTask(_textInput, textItem.Text, _duration, _source.Token);
                CurrentNode = textItem.Node.GetNext();
            }
            else if (item is BranchItem branchItem)
            {
                _view.SetBranchButtonsVisible(true, branchItem.BranchTexts.Length);
                _view.SetTextVisible(false);
                int index = await _view.GetPressedButtonIndexAsync(branchItem.BranchTexts);
                _view.SetBranchButtonsVisible(false, 0);
                _view.SetTextVisible(true);
                
                CurrentNode = branchItem.Node.GetNext(index);

                goto begin;
            }
            else if (item is ExecutionItem executionItem)
            {
                executionItem.Execution();
                CurrentNode = executionItem.Node.NextNode;
                goto begin;
            }
        }
        catch (OperationCanceledException)
        {
            if (item is TextItem textItem)
            {
                _textInput(textItem.Text);
                CurrentNode = textItem.Node.GetNext();
            }
            else if (item is BranchItem branchItem)
            {
                _view.SetBranchButtonsVisible(false, 0);
                _view.SetTextVisible(true);
                CurrentNode = branchItem.Node.GetNext(0);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        IsRunning = false;
        
    }

    public void Cancel()
    {
        _source?.Cancel();
        _source = null;
    }

    public DialogueContext(DialogueRuntimeTree tree, float duration, DialogueView view)
    {
        _tree = tree;
        _textInput = str => view.DialogueText = str;
        _duration = duration;
        _view = view;
        _source = new();
        CurrentNode = tree.EntryPoint;
    }
}