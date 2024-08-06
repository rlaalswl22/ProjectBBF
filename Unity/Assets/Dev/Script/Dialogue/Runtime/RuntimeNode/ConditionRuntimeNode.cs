using System;
using System.Collections.Generic;
using DS.Core;

namespace DS.Runtime
{
    public class ConditionRuntimeNode : DialogueRuntimeNode
    {
        public DialogueRuntimeNode TrueNode { get; private set; }
        public DialogueRuntimeNode FalseNode { get; private set; }

        public override bool IsLeaf => TrueNode is null && FalseNode is null;

        public ConditionRuntimeNode(DialogueRuntimeNode trueNode, DialogueRuntimeNode falseNode)
        {
            TrueNode = trueNode;
            FalseNode = falseNode;
        }

        public override DialogueItem CreateItem()
            => null;

        public DialogueRuntimeNode GetNext()
        {
            throw new Exception();
        }
    }
}