using System;
using DS.Core;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Minigame/FrogRace", fileName = "FrogRace")]
public class FrogRaceMinigameData : MinigameData
{
    [SerializeField, Header("튜토리얼 대사")] private DialogueContainer _tutorial;

    [SerializeField, Header("부스트시 이동거리 배율")]
    private float _boostMovementMultiplier;

    [SerializeField, Header("점프 시도 간격(초)")]
    private float _jumpInterval;

    [SerializeField, Header("점프 최소 거리")] private float _jumpMinDistance;
    [SerializeField, Header("점프 최대 거리")] private float _jumpMaxDistance;

    [SerializeField, Header("점프 속도")] private float _movementSpeed;


    [SerializeField, Header("점프 최대 높이")] private float _jumpMaxHeight;
    
    [SerializeField, Header("개구리 정보")] private FrogData[] _frogs;

    [Serializable]
    public struct FrogData
    {
        public FrogRaceFrogObject FrogObject;
        [Header("개구리 화면 출력 이름")] public string DisplayName;
        [Header("배당률")] public float DividendRate;
        [Header("점프 성공 확률")] public float JumpRate;
        [Header("부스트 성공 확률")] public float JumpBoostRate;
    }

    public DialogueContainer Tutorial => _tutorial;
    

    public float BoostMovementMultiplier => _boostMovementMultiplier;

    public float JumpInterval => _jumpInterval;

    public float JumpMinDistance => _jumpMinDistance;

    public float JumpMaxDistance => _jumpMaxDistance;


    public float MovementSpeed => _movementSpeed;

    public float JumpMaxHeight => _jumpMaxHeight;

    public FrogData[] Frogs => _frogs;
}