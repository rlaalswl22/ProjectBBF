using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmlandController : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    
    [SerializeField] private FarmlandTile _tile;

    private Dictionary<Vector3Int, FarmlandTile> _originTable = new();

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ResetAllTile();
        }

        var pos = WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonDown(0))
        {
            TryCultivateTile(pos, _tile);
        }
        if (Input.GetMouseButtonDown(1))
        {
            ResetTile(pos);
        }
    }

    private void Init()
    {
        BoundsInt bounds = _tilemap.cellBounds;
        for (int i = bounds.x; i < bounds.xMax; i++)
        {
            for (int j = bounds.y; j < bounds.yMax; j++)
            {
                for (int k = bounds.z; k < bounds.zMax; k++)
                {
                    var cellPos = new Vector3Int(i, j, k);
                    var tile = _tilemap.GetTile(cellPos);                    
                    
                    if (tile is not FarmlandTile farmlandTile) continue;

                    _originTable[cellPos] = farmlandTile;
                }

            }
        }
    }

    public List<TileBase> GetAllTiles()
    {
        List<TileBase> tiles = new List<TileBase>();

        BoundsInt bounds = _tilemap.cellBounds;
        TileBase[] allTiles = _tilemap.GetTilesBlock(bounds);

        foreach (TileBase tile in allTiles)
        {
            if (tile != null && !tiles.Contains(tile))
            {
                tiles.Add(tile);
            }
        }

        return tiles;
    }
    
    public List<FieldItem> TryCultivateTile(Vector3Int cellPos, FarmlandTile tile)
    {
        if (CanCultivate(cellPos) == false) return new List<FieldItem>();
        if (tile.IsCultivate == false) return new List<FieldItem>();
        
        _tilemap.SetTile(cellPos, tile);

        var list = new List<FieldItem>();

        for (int i = 0; i < tile.DropItemCount; i++)
        {
            var item = FieldItem.Create(new FieldItem.FieldItemInitParameter
            {
                ItemData = tile.DropItem,
                Position = _tilemap.CellToWorld(cellPos)
            });
            
            list.Add(item);
        }
        
        return list;
    }

    public void ResetTile(Vector3Int cellPos)
    {
        if (_originTable.TryGetValue(cellPos, out var tile))
        {
            _tilemap.SetTile(cellPos, tile);
        }
    }

    public void ResetAllTile()
    {
        foreach (var pair in _originTable)
        {
            Vector3Int cellPos = pair.Key;
            FarmlandTile tile = pair.Value;

            
            _tilemap.SetTile(cellPos, tile);
        }
    }

    public void ForceCultivateTile(Vector3Int cellPos, FarmlandTile tile)
    {
        _tilemap.SetTile(cellPos, tile);
    }

    public Vector3Int WorldToCell(Vector3 worldPos) => _tilemap.WorldToCell(worldPos);

    public bool CanCultivate(Vector3Int cellPos)
    {
        var tile = _tilemap.GetTile<FarmlandTile>(cellPos);

        if (tile is null) return false;
        
        return tile.IsCultivate;
    }
}
