using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class FarmlandTileController : MonoBehaviour
{
    
    [SerializeField] protected Tilemap _platformTilemap;
    [SerializeField] protected Tilemap _plantTilemap;
    
    // testcode
    [SerializeField] private FarmlandTile _tile;
    [SerializeField] private GrownDefinition _definition;

    private Dictionary<Vector3Int, FarmlandTile> _originTable = new();
    private Dictionary<Vector3Int, FarmlandGrownInfo> _grownInfos = new();

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
            if (GetPlantTile(pos))
            {
                ResetPlantTile(pos);
            }
            else
            {
                Plant(pos, _definition);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            DestroyPlantTile(pos, null);
        }
    }

    private void Init()
    {
        BoundsInt bounds = _platformTilemap.cellBounds;
        for (int i = bounds.x; i < bounds.xMax; i++)
        {
            for (int j = bounds.y; j < bounds.yMax; j++)
            {
                for (int k = bounds.z; k < bounds.zMax; k++)
                {
                    var cellPos = new Vector3Int(i, j, k);
                    var tile = _platformTilemap.GetTile(cellPos);                    
                    
                    if (tile is not FarmlandTile farmlandTile) continue;

                    _originTable[cellPos] = farmlandTile;
                }

            }
        }
    }
    
    public bool TryCultivateTile(Vector3Int cellPos, FarmlandTile tile)
    {
        if (CanCultivate(cellPos) == false) return false;
        
        SetTile(cellPos, tile);
        return true;
    }
    
    public List<FieldItem> FarmTile(Vector3Int cellPos)
    {
        var list = new List<FieldItem>();

        var tile = _platformTilemap.GetTile<FarmlandTile>(cellPos);

        if (tile is null) return new List<FieldItem>();

        for (int i = 0; i < tile.DropItemCount; i++)
        {
            var item = FieldItem.Create(new FieldItem.FieldItemInitParameter
            {
                ItemData = tile.DropItem,
                Position = _platformTilemap.CellToWorld(cellPos)
            });
            
            list.Add(item);
        }

        return list;
    }

    public GrowingTile GetPlantTile(Vector3Int cellPos)
    {
        return _plantTilemap.GetTile<GrowingTile>(cellPos);
    }

    [CanBeNull]
    public FarmlandGrownInfo GetGrownInfo(Vector3Int cellPos)
    {
        if (_grownInfos.TryGetValue(cellPos, out var info))
        {
            return info;
        }

        return null;
    }

    public List<FarmlandGrownInfo> GetAllGrownInfo()
        => _grownInfos.Values.ToList();

    public void DestroyPlantTile(Vector3Int cellPos, List<ItemData> list)
    {
        var tile = _plantTilemap.GetTile<GrowingTile>(cellPos);

        if (tile is null) return;

        for (int i = 0; i < tile.DropItemCount; i++)
        {
            // 필드에 아이템 드랍되게 하면 이 코드 사용
            //var r = FieldItem.Create(new FieldItem.FieldItemInitParameter()
            //{
            //    ItemData = tile.DropItem,
            //    Position = _plantTilemap.CellToWorld(cellPos)
            //});

            if (list is not null)
            {
                list.Add(tile.DropItem);
            }
        }
        
        ResetPlantTile(cellPos);
    }
    
    public void ResetPlantTile(Vector3Int cellPos)
    {
        _plantTilemap.SetTile(cellPos, null);
        _grownInfos.Remove(cellPos);
    }
    public void ResetPlatformTile(Vector3Int cellPos)
    {
        if (_originTable.TryGetValue(cellPos, out var tile))
        {
            _platformTilemap.SetTile(cellPos, tile);
        }
        else
        {
            _platformTilemap.SetTile(cellPos, null);
        }
    }

    public void ResetAllTile()
    {
        _platformTilemap.ClearAllTiles();
        _plantTilemap.ClearAllTiles();
        
        foreach (var pair in _originTable)
        {
            Vector3Int cellPos = pair.Key;
            FarmlandTile tile = pair.Value;
            
            _platformTilemap.SetTile(cellPos, tile);
        }
    }

    public void ForceCultivateTile(Vector3Int cellPos, FarmlandTile tile)
    {
        SetTile(cellPos, tile);
    }

    public Vector3Int WorldToCell(Vector3 worldPos) => _platformTilemap.WorldToCell(worldPos);

    public bool CanCultivate(Vector3Int cellPos)
    {
        var tile = _platformTilemap.GetTile<FarmlandTile>(cellPos);

        if (tile is null) return false;
        
        return tile.IsCultivate;
    }

    public bool CanPlant(Vector3Int cellPos)
    {
        var tile = _platformTilemap.GetTile<FarmlandTile>(cellPos);

        if (tile is null) return false;
        
        return tile.IsCultivate == false;
    }

    public void RefeshPlantTile(Vector3Int cellPos)
    {
        var info = GetGrownInfo(cellPos);
        if (info is null) return;

        SetTile(cellPos, info.CurrentTile);
    }
    
    public bool Plant(Vector3Int cellPos, GrownDefinition definition)
    {
        if (CanPlant(cellPos) == false) return false;
        
        if (_grownInfos.TryGetValue(cellPos, out var info))
        {
            return false;
        }
        else
        {
            _grownInfos[cellPos] = new FarmlandGrownInfo(
                definition,
                cellPos
            );
            
            SetTile(cellPos, definition.FirstTile);
        }

        return true;
    }
    private void SetTile(Vector3Int cellPos, FarmlandTile tile)
    {
        Tilemap tilemap = null;
        
        switch (tile)
        {
            case GrowingTile:
                tilemap = _plantTilemap;
                break;
            default: // FarmlandTile case
                tilemap = _platformTilemap;
                break;
        }
        
        Debug.Assert(tilemap is not null);
        
        tilemap.SetTile(cellPos, tile);

    }
}
