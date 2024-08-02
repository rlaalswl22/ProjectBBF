using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ProjectBBF/FarmSystem/Farmland/FarmlandTile", fileName = "NewFarmlandTile")]
public class FarmlandTile : Tile
{
    [SerializeField]
    private bool _isCultivate;

    [SerializeField] private ItemData _dropItem;
    [SerializeField] private int _dropItemCount;

    public bool IsCultivate => _isCultivate;

    public ItemData DropItem => _dropItem;

    public int DropItemCount => _dropItemCount;
}
