using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ProjectBBF/Tool/RuleTimeSpriteSetter", fileName = "RuleSpriteSetter")]
public class RuleTileSpriteSetter : ScriptableObject
{
    [SerializeField] private Tile.ColliderType _colliderType;
    [SerializeField] private RuleTile _ruleTile;
    [SerializeField] private Texture2D _spr;
    
    
    [ButtonMethod]
    private void Set()
    {
        var path = AssetDatabase.GetAssetPath(_spr.GetInstanceID());
        var sprs = AssetDatabase.LoadAllAssetsAtPath(path);
        if (sprs is null) return;
        if (_ruleTile is null) return;
        if (sprs.Length == 0) return;

        int i = 1;
        foreach (var rule in _ruleTile.m_TilingRules)
        {
            if(sprs.Length <= i)return;
            if (sprs[i] is not Sprite spr) continue;

            rule.m_Sprites[0] = spr;
            rule.m_ColliderType = _colliderType;
            i++;
        }

        EditorUtility.SetDirty(_ruleTile);
    }
    
    [ButtonMethod]
    private void Clear()
    {
        if (_ruleTile is null) return;

        foreach (var rule in _ruleTile.m_TilingRules)
        {
            for (int r = 0; r < rule.m_Sprites.Length; r++)
            {
                rule.m_Sprites[r] = null;
            }
        }
        
        EditorUtility.SetDirty(_ruleTile);
    }
}
