using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/CultivationTile", fileName = "NewCultivationTile")]
public class CultivationItemData : ItemData
{
    [SerializeField] private FarmlandTile _farmlandTile;

    public FarmlandTile FarmlandTile => _farmlandTile;
}