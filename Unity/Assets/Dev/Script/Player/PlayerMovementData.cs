using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/Data/Player/MovementData", fileName = "SampleMovementData")]

public class PlayerMovementData : ScriptableObject
{
    [field: SerializeField, Foldout("이동 관련 설정"), OverrideLabel("이동속도(m/s)")]
    private float _movementSpeed;

    [field: SerializeField, InitializationField, Foldout("스텟 관련 설정"), OverrideLabel("최대 체력")]
    private float _maxHealth;

    public float MovementSpeed => _movementSpeed;

    public float MaxHealth => _maxHealth;
}
