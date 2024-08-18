using System.Collections.Generic;
using UnityEngine;

namespace ProjectBBF.Persistence
{
    [CreateAssetMenu(menuName = "ProjectBBF/Project/Persistence", fileName = "Persistence Table")]
    public class PersistenceTable : ScriptableObject
    {

        [SerializeField] private List<string> _list;
        
        public IReadOnlyList<string> Keys => _list;
    }
}