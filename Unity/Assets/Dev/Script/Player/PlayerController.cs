using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField, Foldout("데이터"), InitializationField, OverrideLabel("플레이어 이동 데이터"), MustBeAssigned, DisplayInspector]
    private PlayerMovementData _movementData;
    
    [field: SerializeField, Separator("컴포넌트"), MustBeAssigned, AutoProperty(AutoPropertyMode.Children)]
    private Rigidbody2D _rigidbody;

    [field: SerializeField, MustBeAssigned, AutoProperty(AutoPropertyMode.Children)]
    private Animator _animator;



    #region Getter/Setter
    public PlayerMovementData MovementData => _movementData;
    public Rigidbody2D Rigidbody => _rigidbody;
    public Animator Animator => _animator;
    
    
    public PlayerMove MoveStrategy { get; private set; }
    public PlayerVisual VisualStrategy { get; private set; }
    public PlayerStateTranslator Translator { get; private set; }
    public PlayerCollector Collector { get; private set; }
    #endregion

    private void Awake()
    {
        MoveStrategy = gameObject.AddComponent<PlayerMove>();
        VisualStrategy = gameObject.AddComponent<PlayerVisual>();
        Translator = gameObject.AddComponent<PlayerStateTranslator>();
        Collector = gameObject.AddComponent<PlayerCollector>();
        
        MoveStrategy.Init(this);
        VisualStrategy.Init(this);
        Translator.Init(this);
        Collector.Init(this);
    }

    
    // TODO: dummy 코드,
    private void Update()
    {
        MoveStrategy.OnMove();
    }
}
