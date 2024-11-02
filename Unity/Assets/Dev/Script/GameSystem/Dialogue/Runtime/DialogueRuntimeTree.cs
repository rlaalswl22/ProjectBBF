using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DS.Core;
using DS.Runtime;
using JetBrains.Annotations;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DS.Core
{
    public class DialogueRuntimeTree
    {
        public string DebugDialogueFileName { get; private set; }

        public static DialogueRuntimeTree Build(DialogueContainer container)
        {
            List<DialogueNodeData> datas = container.NodeData;
            List<NodeLinkData> links = container.NodeLinks;

            var firstNodeData = datas.First(x => x.GUID == links.First().TargetNodeGuid);
            
            var currentRuntimeNode = firstNodeData.CreateRuntimeNode(datas, links);

            var tree = new DialogueRuntimeTree();
            tree.EntryPoint = currentRuntimeNode;
            
#if UNITY_EDITOR
            tree.DebugDialogueFileName = $"file name: {container.name}\npath: {AssetDatabase.GetAssetPath(container.GetInstanceID())}";
#endif
            
            Debug.Assert(tree.EntryPoint is not null);

            return tree;
        }
        

        public DialogueRuntimeNode EntryPoint { get; private set; }
    }
}