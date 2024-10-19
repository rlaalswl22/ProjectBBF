using System.Collections.Generic;
using UnityEngine;

namespace ProjectBBF.Persistence
{
    [System.Serializable, GameData]
    public class RecipeBookPersistenceObject
    {
        [SerializeField] private List<string> _unlockRecipeKeys = new(10);

        public List<string> UnlockRecipeKeys => _unlockRecipeKeys;
    }
}