using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField, Foldout("데이터"), OverrideLabel("플레이어 이동 데이터"), MustBeAssigned, DisplayInspector]
    private PlayerMovementData _movementData;
    [field: SerializeField, Foldout("데이터"), OverrideLabel("플레이어 이동 데이터"), MustBeAssigned, DisplayInspector]
    private PlayerAnimationData _animationData;
    
    [field: SerializeField, Separator("컴포넌트"), MustBeAssigned, AutoProperty(AutoPropertyMode.Children)]
    private Rigidbody2D _rigidbody;

    [field: SerializeField, MustBeAssigned, AutoProperty(AutoPropertyMode.Children)]
    private Animator _animator;

    [field: SerializeField, MustBeAssigned, InitializationField, AutoProperty(AutoPropertyMode.Children)]
    private PlayerQuickInventoryController _quickInventory;

    [field: SerializeField] private ItemData _testTool;
    [field: SerializeField] private GrownItemData _testSeed;


    #region Getter/Setter
    public PlayerMovementData MovementData => _movementData;
    public PlayerAnimationData AnimationData => _animationData;
    public Rigidbody2D Rigidbody => _rigidbody;
    public Animator Animator => _animator;
    
    
    public PlayerMove MoveStrategy { get; private set; }
    public PlayerVisual VisualStrategy { get; private set; }
    public PlayerStateTranslator Translator { get; private set; }
    public PlayerInteracter Interactor { get; private set; }
    public PlayerCoordinate Coordinate { get; private set; }
    
    
    public GridInventory Inventory { get; private set; }
    public PlayerQuickInventoryController QuickInventory => _quickInventory;
    #endregion

    private void Awake()
    {
        MoveStrategy = Bind<PlayerMove>();
        VisualStrategy =Bind<PlayerVisual>();
        Translator = Bind<PlayerStateTranslator>();
        Interactor = Bind<PlayerInteracter>();
        Coordinate = Bind<PlayerCoordinate>();
        
        
        Inventory = new GridInventory(new Vector2Int(10, 3));
        QuickInventory.Init(this);

        Inventory.PushItem(_testTool, 1);
        Inventory.PushItem(_testSeed, 4);
    }

    private T Bind<T>() where T : MonoBehaviour, IPlayerStrategy
    {
        var obj = gameObject.AddComponent<T>();
        obj.Init(this);

        return obj;
    }
    
}
