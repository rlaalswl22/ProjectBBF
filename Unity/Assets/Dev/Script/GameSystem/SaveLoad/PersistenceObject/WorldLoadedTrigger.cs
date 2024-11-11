using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectBBF.Persistence
{
    [Serializable, GameData]
    public class WorldLoadedTriggerPersistenceObject
    {
        [SerializeField] private List<string> _keys = new();

        public List<string> Keys => _keys;
    }
}