using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ProjectBBF/CultivationTile", fileName = "NewCultivationTile")]
public class CultivationItemData : ItemData
{
    [FormerlySerializedAs("_farmlandTile")] [SerializeField] private CultivationTile _cultivationTile;

    public CultivationTile CultivationTile => _cultivationTile;
}