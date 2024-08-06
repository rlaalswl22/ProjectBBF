using System;
using System.Collections.Generic;
using System.Linq;
using DS.Core;
using UnityEngine;

namespace DS.Runtime
{
    [Serializable]
    public class TextNodeData : DialogueNodeData
    {
        public string DialogueText;

        public override DialogueNodeData Clone()
        {
            var data = new TextNodeData();

            data.GUID = GUID;
            data.NodeTitle = NodeTitle;
            data.TypeName = TypeName;
            data.Position = Position;
            
            data.DialogueText = DialogueText;

            return data;
        }

        public override DialogueRuntimeNode CreateRuntimeNode(List<DialogueNodeData> datas, List<NodeLinkData> links)
        {
            NodeLinkData result = links.FirstOrDefault(x => x.BaseNodeGuid == GUID);
            if (result is null) return new TextRuntimeNode(DialogueText, null);

            DialogueNodeData nextData = datas.FirstOrDefault(x => x.GUID == result.TargetNodeGuid);
            if (nextData is null) return new TextRuntimeNode(DialogueText, null);

            DialogueRuntimeNode nextNode = nextData.CreateRuntimeNode(datas, links);

            return new TextRuntimeNode(DialogueText, nextNode);
        }

        public override bool IsEqual(DialogueNodeData other)
        {
            if (!base.IsEqual(other))
                return false;

            var otherNode = other as TextNodeData;
            
            if (DialogueText != otherNode.DialogueText)
                return false;
            
            return true;
        }
    }

}