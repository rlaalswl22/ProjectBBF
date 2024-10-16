using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/ContestResult", fileName = "New ContestResult")]
public class ContestResultData : ScriptableObject
{
    [Serializable]
    public struct Record
    {
        [Tooltip("0: 프롤로그, 1~4: 챕터, 그 이외는 정의되지 않음")]
        public int Chapter;
        public ItemData Item;
        public string ActorKey;
        public string Text;
    }

    [SerializeField] private List<Record> _table;

    public IReadOnlyList<Record> Table => _table;
}