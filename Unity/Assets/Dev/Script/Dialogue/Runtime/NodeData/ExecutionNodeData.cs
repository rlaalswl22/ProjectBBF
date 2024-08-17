using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DS.Core;
using UnityEngine;

namespace DS.Runtime
{
    [Serializable]
    public class ExecutionNodeData : ParameterNodeData
    {
        public override DialogueNodeData Clone()
        {
            var data = new ExecutionNodeData();

            data.GUID = GUID;
            data.NodeTitle = NodeTitle;
            data.TypeName = TypeName;
            data.Position = Position;

            data.Handler = Handler;
            data.Warps = Warps;

            return data;
        }

        public override DialogueRuntimeNode CreateRuntimeNode(List<DialogueNodeData> datas, List<NodeLinkData> links)
        {
            var data = datas.FirstOrDefault(x =>
            {
                var link = links.FirstOrDefault(y => y.BaseNodeGuid == GUID);
                Debug.Assert(link is not null);

                return x.GUID == link.TargetNodeGuid;
            });
            
            Debug.Assert(data is not null);

            var runtimeNode = data.CreateRuntimeNode(datas, links);
            return new ExecutionRuntimeNode(runtimeNode, Handler, GetArgs());
        }
            
        public override bool IsEqual(DialogueNodeData other)
        {
            if (!base.IsEqual(other))
                return false;
            
            return true;
        }
    }
}