using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

[CustomEditor(typeof(FarmlandTile))]
public class FarmlandTileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FarmlandTile obj = (FarmlandTile)target;

        EditorGUILayout.LabelField("기본 Tile 정보");
        
        obj.sprite = EditorGUILayout.ObjectField("Sprite", obj.sprite, typeof(Sprite), false) as Sprite;
        obj.color = EditorGUILayout.ColorField("Color", obj.color);
        obj.colliderType = (Tile.ColliderType)EditorGUILayout.EnumPopup("ColliderType", obj.colliderType);



        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Farmland Tile 정보");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_isCultivate")); 
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_dropItem")); 
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_dropItemCount")); 
        
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(obj);
            serializedObject.ApplyModifiedProperties();
        }
        
        
    }
    

    private static Type GetType(string typeName)
    {
        var type = Type.GetType(typeName);
        if (type != null)
            return type;

        var currentAssembly = Assembly.GetExecutingAssembly();
        var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        foreach (var assemblyName in referencedAssemblies)
        {
            var assembly = Assembly.Load(assemblyName);
            if (assembly != null)
            {
                type = assembly.GetType(typeName);
                if (type != null)
                    return type;
            }
        }
        return null;
    }
    
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        
        FarmlandTile tile = (FarmlandTile)target;
        
        if (tile.sprite != null)
        {
            Type t = GetType("UnityEditor.SpriteUtility");
            
            if (t != null)
            {
                MethodInfo method = t.GetMethod("RenderStaticPreview", new[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) });
                if (method != null)
                {
                    object ret = method.Invoke("RenderStaticPreview", new object[] { tile.sprite, Color.white, width, height });
                    if (ret is Texture2D)
                        return ret as Texture2D;
                }
            }
        }
        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }
}
