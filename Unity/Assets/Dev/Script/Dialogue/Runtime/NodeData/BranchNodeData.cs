using System;
using System.Collections.Generic;
using System.Linq;
using DS.Core;
using UnityEngine;

namespace DS.Runtime
{
    [Serializable]
    public class BranchNodeData : DialogueNodeData
    {
        public override DialogueNodeData Clone()
        {
            var data = new BranchNodeData();

            data.GUID = GUID;
            data.NodeTitle = NodeTitle;
            data.TypeName = TypeName;
            data.Position = Position;
            
            return data;
        }

        public override DialogueRuntimeNode CreateRuntimeNode(List<DialogueNodeData> datas, List<NodeLinkData> links)
        {
            var nextNodes = links
                .Where(x => x.BaseNodeGuid == GUID)
                .Select(x =>
                {
                    DialogueNodeData data = datas.FirstOrDefault(y => y.GUID == x.TargetNodeGuid);
                    Debug.Assert(data is not null);

                    DialogueRuntimeNode nextNode = data.CreateRuntimeNode(datas, links);
                    return new BranchRuntimeNode.BranchItem(nextNode, x.PortName);
                })
                .ToList();

            return new BranchRuntimeNode(nextNodes);
        }

        public override bool IsEqual(DialogueNodeData other)
        {
            if (!base.IsEqual(other))
                return false;

            var otherNode = other as BranchNodeData;
            
            return true;
        }
    }
}