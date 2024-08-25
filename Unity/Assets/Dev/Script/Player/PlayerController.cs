using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using ProjectBBF.Event;
using ProjectBBF.Persistence;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    [field: SerializeField, MustBeAssigned, AutoProperty(AutoPropertyMode.Children)]
    private StateTransitionHandler _stateHandler;
    [field: SerializeField, MustBeAssigned, AutoProperty(AutoPropertyMode.Children)]
    private CollisionInteraction _interaction;

    [field: SerializeField, MustBeAssigned, InitializationField, AutoProperty(AutoPropertyMode.Children)]
    private PlayerQuickInventoryController _quickInventory;

    [field: SerializeField] private ItemData _testTool;
    [field: SerializeField] private ItemData _testWaterSpray;
    [field: SerializeField] private PlantItemData _testSeed;
    [field: SerializeField] private FertilizerItemData _testFertilizer;
    [field: SerializeField] private Vector2 _interactionOffset;
    [field: SerializeField] private Vector2 _interactionDirFactor;
    [field: SerializeField] private float _interactionRadius;

    public Vector2 InteractionOffset => _interactionOffset;
    public float InteractionRadius => _interactionRadius;

    public Vector2 InteractionDirFactor => _interactionDirFactor;

    public StateTransitionHandler StateHandler => _stateHandler;

    public CollisionInteraction Interaction => _interaction;
    public PlayerBlackboard Blackboard { get; private set; }


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
        
        GameObjectStorage.Instance.AddGameObject(gameObject);


        var info = ActorContractInfo.Create(() => gameObject);
        Interaction.SetContractInfo(info, this);
        
        StateHandler.Init(Interaction);

        DataInit();
    }

    private void DataInit()
    {
        Blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");

        Blackboard.MaxStemina = _movementData.DefaultStemina;
        Blackboard.Stemina = _movementData.DefaultStemina;
    }
    
    private void OnDestroy()
    {
        if (GameObjectStorage.Instance)
        {
            GameObjectStorage.Instance.RemoveGameObject(gameObject);
        }
    }

    private void Start()
    {
        Inventory.PushItem(_testTool, 1);
        Inventory.PushItem(_testWaterSpray, 1);
        Inventory.PushItem(_testSeed, 4);
        Inventory.PushItem(_testFertilizer, 4);
    }

    private T Bind<T>() where T : MonoBehaviour, IPlayerStrategy
    {
        var obj = gameObject.AddComponent<T>();
        obj.Init(this);

        return obj;
    }
    
}
