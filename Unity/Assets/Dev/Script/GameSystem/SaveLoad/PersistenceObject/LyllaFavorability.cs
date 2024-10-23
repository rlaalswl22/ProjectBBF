using System.Collections.Generic;
using UnityEngine.Serialization;

namespace ProjectBBF.Persistence
{
    [System.Serializable, GameData]
    public class LyllaFavorabilityPersistenceObject
    {
        public Dictionary<string, int> _indexTable = new();
        public HashSet<string> _tutorialDialogueOnceTable = new();
        public bool MoveNextLock = false;
    }
}