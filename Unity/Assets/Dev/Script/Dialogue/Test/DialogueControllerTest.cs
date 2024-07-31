using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DS.Core;
using DS.Runtime;
using MyBox;
using UnityEngine;

public class DialogueControllerTest : MonoBehaviour
{
    public DialogueController Controller;
    public DialogueContainer Container;

    private DialogueContext _context;
    
    private void CreateContext()
    {
        if (Application.isPlaying == false) return;
        if (Controller == false) return;

        _context = Controller.CreateContext(DialogueRuntimeTree.Build(Container));
    }

    private void Start()
    {
        Controller.Visible = true;
        CreateContext();
    }

    private void Update()
    {
        if (_context == null) return;

        // TODO: inputaction 설정하지 않아서 일단 주석처리. 향후 변경된 트리거로 변경하고 코드 활성화 바람
        //if (InputManager.Actions.DialogueSkip.triggered)
        //{
        //    _context.Next();
        //    if (_context.CanNext == false)
        //    {
        //        _context = null;
        //        CreateContext();
        //    }
        //}
    }
}
