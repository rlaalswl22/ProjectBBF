using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CloudController))]
public class CloudControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 대상 오브젝트 가져오기
        CloudController controller = (CloudController)target;

        // 기본 인스펙터 드로잉
        DrawDefaultInspector();

        // 슬라이더 범위 표시
        controller.SliderValue = EditorGUILayout.Slider("Slider Value", controller.SliderValue, 0f, controller.Distance);

        // 값이 변경되었는지 확인하여 적용
        if (GUI.changed)
        {
            EditorUtility.SetDirty(controller);
            controller.OnEditorUpdate();
        }
    }
}