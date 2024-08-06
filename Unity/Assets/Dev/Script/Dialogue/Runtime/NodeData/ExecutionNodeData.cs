using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DS.Core;
using UnityEngine;

namespace DS.Runtime
{
    [Serializable]
    public class ExecutionNodeData : DialogueNodeData
    {
        [Serializable]
        public class Warp
        {
            public string Target;
            public int IntValue;
            public float FloatValue;
            public string StringValue;
            
            public bool IsEqual(Warp other)
            {
                if (Target != other.Target) return false;
                if (IntValue != other.IntValue) return false;
                if (FloatValue != other.FloatValue) return false;
                if (StringValue != other.StringValue) return false;
                
                return true;
            }
        }
        
        public ExecutionDescriptor Descriptor;
        public Warp[] Warps = new Warp[]{};
        
        public override DialogueNodeData Clone()
        {
            var data = new ExecutionNodeData();

            data.GUID = GUID;
            data.NodeTitle = NodeTitle;
            data.TypeName = TypeName;
            data.Position = Position;

            data.Descriptor = Descriptor;
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
            return new ExecutionRuntimeNode(runtimeNode, Descriptor, Warps.Select(x =>
            {
                switch (x.Target)
                {
                    case "Int":
                        return (object)x.IntValue;
                    case "Float":
                        return x.FloatValue;
                    case "String":
                        return x.StringValue;
                    default:
                        return "ERROR";
                }
            }).ToArray());
        }
            
        public override bool IsEqual(DialogueNodeData other)
        {
            if (!base.IsEqual(other))
                return false;

            var otherNode = other as ExecutionNodeData;

            if (Descriptor != otherNode.Descriptor) return false;
            if (Warps.Length != otherNode.Warps.Length) return false;

            for (int i = 0; i < Warps.Length; i++)
            {
                if (Warps[i].IsEqual(otherNode.Warps[i]) == false) return false;
            }
            
            return true;
        }
    }
}