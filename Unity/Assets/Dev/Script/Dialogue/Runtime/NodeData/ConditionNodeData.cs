using System;
using System.Collections.Generic;
using System.Linq;
using DS.Core;
using UnityEngine;

namespace DS.Runtime
{
    
    [Serializable]
    public class ConditionNodeData : DialogueNodeData
    {
        public string ItemID;
        public int EqualOrMany;

        public override DialogueNodeData Clone()
        {
            var data = new ConditionNodeData();

            data.GUID = GUID;
            data.NodeTitle = NodeTitle;
            data.TypeName = TypeName;
            data.Position = Position;

            data.ItemID = ItemID;
            data.EqualOrMany = EqualOrMany;

            return data;
        }

        public override DialogueRuntimeNode CreateRuntimeNode(List<DialogueNodeData> datas, List<NodeLinkData> links)
        {
            var nextNodes = links
                .Where(x => x.BaseNodeGuid == GUID);
            
            
            DialogueRuntimeNode trueNextNode = null;
            DialogueRuntimeNode falseNextNode = null;

            foreach (NodeLinkData x in nextNodes)
            {
                DialogueNodeData data = datas.FirstOrDefault(y => y.GUID == x.TargetNodeGuid && x.PortName is "True" or "False");
                Debug.Assert(data is not null);

                var node = data.CreateRuntimeNode(datas, links);

                if (x.PortName == "True")
                {
                    trueNextNode = node;
                }
                else
                {
                    falseNextNode = node;
                }
            }
            
            Debug.Assert(trueNextNode is not null);
            Debug.Assert(falseNextNode is not null);

            return new ConditionRuntimeNode(trueNextNode, falseNextNode);
        }

        public override bool IsEqual(DialogueNodeData other)
        {
            if (!base.IsEqual(other))
                return false;

            var otherNode = other as ConditionNodeData;
            
            if (ItemID != otherNode.ItemID)
                return false;
            
            if (EqualOrMany != otherNode.EqualOrMany)
                return false;

            return true;
        }
    }
}