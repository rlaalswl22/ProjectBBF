using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*
https://bonnate.tistory.com/

Insert the script into the game object
insert the TMP font in the inspector
and press the button to find and replace all components.

It may work abnormally, so make sure to back up your scene before using it!!
*/

public class TMP_FontChanger : MonoBehaviour
{
    [SerializeField] public TMP_FontAsset FontAsset;
    [SerializeField] public bool FindChild;
}

#if UNITY_EDITOR
[CustomEditor(typeof(TMP_FontChanger))]
public class TMP_FontChangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Change Font!"))
        {
            TMP_FontAsset fontAsset = ((TMP_FontChanger)target).FontAsset;

            List<TextMeshPro> list3d = null;
            List<TextMeshProUGUI> listui = null;
            
            if (((TMP_FontChanger)target).FindChild)
            {
                list3d = ((TMP_FontChanger)target).gameObject.GetComponentsInChildren<TextMeshPro>(true).ToList();
                listui = ((TMP_FontChanger)target).gameObject.GetComponentsInChildren<TextMeshProUGUI>(true).ToList();
            }
            else
            {
                list3d = GameObject.FindObjectsOfType<TextMeshPro>(true).ToList();
                listui = GameObject.FindObjectsOfType<TextMeshProUGUI>(true).ToList();
            }
            
                
            foreach(TextMeshPro textMeshPro3D in list3d) 
            { 
                textMeshPro3D.font = fontAsset;
            }
            foreach(TextMeshProUGUI textMeshProUi in listui) 
            { 
                textMeshProUi.font = fontAsset;
            }
        }
    }
}
#endif