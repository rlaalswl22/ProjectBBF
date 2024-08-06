using System;
using System.Collections.Generic;
using DS.Core;

namespace DS.Runtime
{
    public class ExecutionRuntimeNode : DialogueRuntimeNode
    {
        public override bool IsLeaf => NextNode is null;
        public readonly DialogueRuntimeNode NextNode;
        public readonly ExecutionDescriptor Descriptor;
        public readonly object[] Arguments;

        public ExecutionRuntimeNode(DialogueRuntimeNode nextNode, ExecutionDescriptor descriptor, object[] arguments)
        {
            NextNode = nextNode;
            Descriptor = descriptor;
            Arguments = arguments;
        }

        public override DialogueItem CreateItem()
            => new ExecutionItem(this, () => Descriptor.Execute(Arguments), "Default", "None");
    }
}