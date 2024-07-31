using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/InteractiveObject/CollectingObjectData", fileName = "CollectingObjectData")]
public class CollectingObjectData : ScriptableObject
{
    [field: SerializeField, OverrideLabel("채집 시간(초)")] 
    private float _collectingTime;
    [field: SerializeField, OverrideLabel("불에 타기까지 걸리는 시간(초)")] 
    private float _buringTime;
    [field: SerializeField, OverrideLabel("플레이어의 채집 애니메이션")] 
    private EPlayerCollectingAnimation _playerAnimationType;

    public float CollectingTime => _collectingTime;
    public float BuringTime => _buringTime;
    public EPlayerCollectingAnimation PlayerAnimationType => _playerAnimationType;
}
