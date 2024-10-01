


using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Bakery/Pressed", fileName = "PressedData")]
public class BakeryPressedData : ScriptableObject
{
    [SerializeField] private float _pressDuration;

    public float PressDuration => _pressDuration;
}