using System;

namespace DS.Runtime
{
    public class ExecutionItem : DialogueItemT<ExecutionRuntimeNode>
    {
        public readonly Action Execution;
        
        public ExecutionItem(ExecutionRuntimeNode node, Action execution, string characterDisplayName, string portraitKey) : base(node, characterDisplayName, portraitKey)
        {
            Execution = execution;
        }
    }
}