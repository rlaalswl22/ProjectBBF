using System;
using System.Collections.Generic;
using UnityEngine;
using DS.Core;


#if UNITY_EDITOR
using UnityEditor;
#endif


namespace DS.Core
{
    [Serializable]
    public class DialogueContainer : ScriptableObject
    {
        [field: SerializeField, HideInInspector]
        private string _guid = string.Empty;
        public string Guid => _guid;
    
#if UNITY_EDITOR
        private void OnEnable()
        {
            if (string.IsNullOrEmpty(_guid))
            {
                _guid = GUID.Generate().ToString();
            }
        }
#endif
        
        public List<NodeLinkData> NodeLinks = new();
        
        [SerializeReference]
        public List<DialogueNodeData> NodeData = new();
        
        public bool IsEqual(DialogueContainer other)
        {
            if (Guid != other.Guid) return false;
            
            // NodeLinks 리스트 비교
            if (NodeLinks.Count != other.NodeLinks.Count)
                return false;

            for (int i = 0; i < NodeLinks.Count; i++)
            {
                if (!NodeLinks[i].IsEqual(other.NodeLinks[i])) 
                    return false;
            }

            // DialogueNodeData 리스트 비교
            if (NodeData.Count != other.NodeData.Count)
                return false;

            for (int i = 0; i < NodeData.Count; i++)
            {
                if (NodeData[i].TypeName != other.NodeData[i].TypeName)
                    return false;

                if (!NodeData[i].IsEqual(other.NodeData[i]))
                    return false;
            }

            return true;
        }
    }
}
