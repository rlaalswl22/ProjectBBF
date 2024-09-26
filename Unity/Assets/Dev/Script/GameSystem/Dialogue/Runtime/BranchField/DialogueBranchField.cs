using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DS.Runtime
{
    public abstract class DialogueBranchResult
    {
    }

    public abstract class DialogueBranchField : MonoBehaviour
    {
        public abstract void DestroySelf();
    }
    
    public interface IDialogueBranchFieldOption{}

    public interface IChooseable : IDialogueBranchFieldOption
    {
        public UniTask<DialogueBranchResult> GetResult(CancellationToken token = default);
    }

    public interface IValuable<out T> : IDialogueBranchFieldOption where T : DialogueBranchResult
    {
        public T GetResult();
    }
}