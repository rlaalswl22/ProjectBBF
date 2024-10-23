

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectBBF.Persistence
{
    [System.Serializable, GameData]
    public class DoOnceHandlerPersistenceObject
    {
        [SerializeField, PersistenceList] private List<string> _doOnceList = new List<string>();
        public List<string> DoOnceList => _doOnceList;
    }
}