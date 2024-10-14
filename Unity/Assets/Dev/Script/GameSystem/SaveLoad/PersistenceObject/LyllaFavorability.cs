using System.Collections.Generic;
using UnityEngine.Serialization;

namespace ProjectBBF.Persistence
{
    [System.Serializable]
    public class LyllaFavorabilityPersistenceObject
    {
        public Dictionary<string, int> _indexTable = new();
        [FormerlySerializedAs("CanMoveNext")] public bool MoveNextLock = true;
    }
}