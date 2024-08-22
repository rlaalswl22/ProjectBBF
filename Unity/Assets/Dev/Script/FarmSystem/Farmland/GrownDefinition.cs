using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ProjectBBF/FarmSystem/Farmland/GrownDefinition", fileName = "NewGrownDefinition")]
public class GrownDefinition : ScriptableObject
{
    [System.Serializable]
    public class GrowingSet
    {
        public PlantTile Tile;
        public int NeedStep;
    }
    
    [SerializeField] private List<GrowingSet> _needGrowingToNextGrowingStep;
    

    public PlantTile FirstTile => NeedGrowingToNextGrowingStep[0]?.Tile;
    public IReadOnlyList<GrowingSet> NeedGrowingToNextGrowingStep
    {
        get
        {
            if (_needGrowingToNextGrowingStep is null)
            {
                return new List<GrowingSet>();
            }

            return _needGrowingToNextGrowingStep;
        }
    }
    
}
