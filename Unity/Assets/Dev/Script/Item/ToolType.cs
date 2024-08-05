using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum ToolType
{
    Absolute,
    Hoe
}

public enum ToolRank : int
{
    R1 = 1,
    R2,
    R3,
    R4,
    R5,
    
    Min = R1,
    Max = R5,
}

[System.Serializable]
public class ToolRequireSet
{
    [SerializeField] private ToolType _requireToolTypes;
    [SerializeField] private ToolRank _requireToolRank;

    public ToolType RequireToolType => _requireToolTypes;
    public ToolRank RequireToolRank => _requireToolRank;

    public ToolRequireSet(ToolType requireToolTypes, ToolRank requireToolRank)
    {
        _requireToolTypes = requireToolTypes;
        _requireToolRank = requireToolRank;
    }
}

[System.Serializable]
public class ItemTypeInfo
{
    [SerializeField] private ToolRequireSet[] _sets;

    public ToolRequireSet[] Sets => _sets;

    public static readonly ItemTypeInfo Empty = new ItemTypeInfo()
    {
        _sets = new ToolRequireSet[0]
    };
    public static readonly ItemTypeInfo GodItemType = new ItemTypeInfo()
    {
        _sets = new []
        {
            new ToolRequireSet(ToolType.Absolute, ToolRank.Max)
        }
    };

    public ItemTypeInfo(ToolRequireSet[] sets)
    {
        _sets = sets;
    }

    public ItemTypeInfo()
    {
    }
    
    
    public bool Contains(ToolType type, ToolRank? rank = null)
        => ToolTypeUtil.Contains(this, type, rank);

    public bool IsCompetible(ItemTypeInfo target)
        => ToolTypeUtil.IsCompetible(this, target);

    public bool IsCompetible(ToolType type, ToolRank rank)
    {
        foreach (ToolRequireSet set in _sets)
        {
            if (ToolTypeUtil.IsCompetible(set.RequireToolType, set.RequireToolRank, type, rank))
            {
                return true;
            }
        }

        return false;
    }
}

public static class ToolTypeUtil
{
    public static bool Contains(ItemTypeInfo info, ToolType type, ToolRank? rank = null)
    {
        foreach (ToolRequireSet set in info.Sets)
        {
            if (set.RequireToolType == type)
            {
                if (rank is null) return true;

                return set.RequireToolRank == rank;
            }
        }

        return false;
    }
    
    public static bool IsCompetible(ToolType current, ToolRank requireRank, ToolType target, ToolRank targetRank)
    {
        if (target is ToolType.Absolute) return true;

        return current == target && (int)requireRank <= (int)targetRank;
    }

    public static bool IsCompetible(ItemTypeInfo current, ItemTypeInfo target)
    {
        return IsCompetible(current.Sets, target.Sets);
    }

    public static bool IsCompetible(ToolRequireSet[] current, ToolRequireSet[] target)
    {
        var targetEnum = target.GetEnumerator();
        
        while (targetEnum.MoveNext())
        {
            if (targetEnum.Current is ToolRequireSet { RequireToolType: ToolType.Absolute })
            {
                return true;
            }
            
            var currentEnum = current.GetEnumerator();

            while (currentEnum.MoveNext())
            {
                if (currentEnum.Current is ToolRequireSet currentSet &&
                    targetEnum.Current is ToolRequireSet targetSet &&
                    IsCompetible(
                        currentSet.RequireToolType,
                        currentSet.RequireToolRank, 
                        targetSet.RequireToolType, 
                        targetSet.RequireToolRank))
                {
                    return true;
                }
            }
        }

        return false;
    }
}