using DS.Core;

namespace DS.Runtime
{
    public class TextRuntimeNode : DialogueRuntimeNode
    {
        public string Text { get; private set; }
        public DialogueRuntimeNode NextNode { get; private set; }
        public override bool IsLeaf => NextNode is null;

        public TextRuntimeNode(string text, DialogueRuntimeNode nextNode)
        {
            Text = text;
            NextNode = nextNode;
        }

        public override DialogueItem CreateItem()
        {
            return new TextItem(this, Text, "default", "None");
        }

        public DialogueRuntimeNode GetNext()
            => NextNode;
    }
}