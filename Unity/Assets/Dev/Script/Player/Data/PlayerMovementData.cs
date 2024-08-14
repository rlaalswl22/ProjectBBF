using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Player/MovementData", fileName = "SampleMovementData")]

public class PlayerMovementData : ScriptableObject
{
    [field: SerializeField, Foldout("이동 관련 설정"), OverrideLabel("이동속도(m/s)")]
    private float _movementSpeed;

    public float MovementSpeed => _movementSpeed;
}
