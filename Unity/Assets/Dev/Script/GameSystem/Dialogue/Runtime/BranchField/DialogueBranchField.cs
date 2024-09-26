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
        public abstract UniTask<DialogueBranchResult> GetResult(CancellationToken token = default);
        public abstract void DestroySelf();
    }
}