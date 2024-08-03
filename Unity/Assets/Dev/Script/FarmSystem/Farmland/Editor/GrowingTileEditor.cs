
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(GrowingTile))]
public class GrowingTileEditor : FarmlandTileEditor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //EditorGUILayout.PropertyField(serializedObject.FindProperty("_growingSprites"));

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
