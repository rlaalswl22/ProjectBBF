namespace DS.Runtime
{
    public class BranchItem : DialogueItemT<BranchRuntimeNode>
    {
        public readonly string[] BranchTexts;

        public BranchItem(BranchRuntimeNode node, string characterDisplayName, string portraitKey, string[] branchTexts) : base(node, characterDisplayName, portraitKey)
        {
            this.BranchTexts = branchTexts;
        }
    }
}