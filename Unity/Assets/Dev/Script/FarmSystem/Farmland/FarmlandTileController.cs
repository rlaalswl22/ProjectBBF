using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class FarmlandTileController : MonoBehaviour, IBODestoryTile, IBOCultivateTile, IBOPlantTile
{
    [SerializeField] protected CollisionInteraction _interaction;
    [SerializeField] protected Tilemap _platformTilemap;
    [SerializeField] protected Tilemap _plantTilemap;
    [SerializeField] protected FarmlandTile _defaultCultivationTile;

    private Dictionary<Vector3Int, FarmlandTile> _originTable = new();
    private Dictionary<Vector3Int, FarmlandGrownInfo> _grownInfos = new();

    private void Awake()
    {
        Init();

        var info = ObjectContractInfo.Create(() => gameObject);
        _interaction.SetContractInfo(info, this);

        info.AddBehaivour<IBODestoryTile>(this);
        info.AddBehaivour<IBOCultivateTile>(this);
        info.AddBehaivour<IBOPlantTile>(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ResetAllTile();
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

        if (tile is null)
        {
            tile = _defaultCultivationTile;
        }
        
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

    public bool DestroyPlantTile(Vector3Int cellPos, ItemTypeInfo itemTypeInfo, List<ItemData> list)
    {
        var tile = _plantTilemap.GetTile<GrowingTile>(cellPos);

        if (tile is null) return false;

        if (ToolTypeUtil.IsCompetible(tile.RequireTools, itemTypeInfo.Sets) == false)
        {
            return false;
        }

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

        return true;
    }

    public bool DestroyPlatformTile(Vector3Int cellPos, ItemTypeInfo itemTypeInfo, List<ItemData> list)
    {
        var tile = _platformTilemap.GetTile<FarmlandTile>(cellPos);

        if (tile is null) return false;

        if (ToolTypeUtil.IsCompetible(tile.RequireTools, itemTypeInfo.Sets) == false)
        {
            return false;
        }

        for (int i = 0; i < tile.DropItemCount; i++)
        {
            // 필드에 아이템 드랍되게 하면 이 코드 사용
            //var r = FieldItem.Create(new FieldItem.FieldItemInitParameter()
            //{
            //    ItemData = tile.DropItem,
            //    Position = _plantTilemap.CellToWorld(cellPos)
            //});

            if (list is not null && tile.DropItem is not null)
            {
                list.Add(tile.DropItem);
            }
        }
        
        ResetPlatformTile(cellPos);
        return true;
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

    public CollisionInteraction Interaction => _interaction;

    public bool TryCultivateTile(Vector3 worldPos, FarmlandTile tile)
        => TryCultivateTile(WorldToCell(worldPos), tile);

    public bool Plant(Vector3 worldPos, GrownDefinition definition)
        => Plant(WorldToCell(worldPos), definition);

    /// 
    [CanBeNull]
    public List<ItemData> Destory(Vector3 worldPos, ItemTypeInfo itemTypeInfo)
    {
        FarmlandTile tile = null;
        Vector3Int cellPos = Vector3Int.zero;
        List<ItemData> list = new List<ItemData>(2);
        
        cellPos = _plantTilemap.WorldToCell(worldPos);
        tile = _plantTilemap.GetTile<FarmlandTile>(cellPos);
        

        if (tile is not null)
        {
            if (DestroyPlantTile(cellPos, itemTypeInfo, list) == false)
            {
                return null;
            }
            
            return list;
        }
        
        cellPos = _platformTilemap.WorldToCell(worldPos);
        tile = _platformTilemap.GetTile<FarmlandTile>(cellPos);

        if (tile is not null)
        {
            if (DestroyPlatformTile(cellPos, itemTypeInfo, list) == false)
            {
                return null;
            }
            
            return list;
        }

        return list;
    }
}
