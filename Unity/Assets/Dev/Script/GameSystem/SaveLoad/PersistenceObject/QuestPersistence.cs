using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectBBF.Persistence
{
    [Serializable, GameData]
    public class QuestPersistence: ISaveLoadNotification
    {
        [Serializable]
        public struct Set
        {
            public string Key;
            public QuestType Type;
        }
        
        [SerializeField] private List<Set> _questKeyList;

        public Dictionary<string, QuestType> QuestTable = new();

        public void OnSavedNotify()
        {
            if (QuestTable is null) return;
            _questKeyList = QuestTable.Select(x => new Set()
            {
                Key = x.Key,
                Type = x.Value
            }).ToList();
        }

        public void OnLoadedNotify()
        {
            if (_questKeyList is null) return;

            var enumerator = _questKeyList.Select(x => new KeyValuePair<string, QuestType>(x.Key, x.Type));
            QuestTable = new(enumerator);
        }
    }
}