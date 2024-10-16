using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/ContestResult", fileName = "New ContestResult")]
public class ContestResultData : ScriptableObject
{
    [Serializable]
    public struct Record
    {
        public ItemData Item;
        public string ActorKey;
        public string Text;
    }

    [SerializeField] private List<Record> _table;

    public IReadOnlyList<Record> Table => _table;
}