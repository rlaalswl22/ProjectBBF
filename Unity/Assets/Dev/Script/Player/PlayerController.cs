using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private PlayerQuickInventoryView _quickInventoryView;

    [field: SerializeField, MustBeAssigned, InitializationField, AutoProperty(AutoPropertyMode.Children)]
    private PlayerMainInventoryView _mainInventoryView;

    [field: SerializeField, MustBeAssigned, InitializationField, AutoProperty(AutoPropertyMode.Children)]
    private PlayerPannelController _pannelController;

    [field: SerializeField, MustBeAssigned, InitializationField, AutoProperty(AutoPropertyMode.Children)]
    private PlayerFishing _fishing;

    [field: SerializeField] private List<ItemDataSerializedSet> _testItems;
    [field: SerializeField] private Vector2 _interactionOffset;
    [field: SerializeField] private Vector2 _interactionDirFactor;
    [field: SerializeField] private float _interactionRadius;

    public Vector2 InteractionOffset => _interactionOffset;
    public float InteractionRadius => _interactionRadius;

    public Vector2 InteractionDirFactor => _interactionDirFactor;

    public StateTransitionHandler StateHandler => _stateHandler;

    public CollisionInteraction Interaction => _interaction;
    public PlayerBlackboard Blackboard { get; private set; }
    private HudController _hudController;

    public HudController HudController
    {
        get
        {
            if (_hudController)
            {
                return _hudController;
            }

            GameObjectStorage.Instance.ForEach(obj =>
            {
                if (obj.gameObject.TryGetComponent<HudController>(out var com))
                {
                    _hudController = com;
                    return false;
                }

                return true;
            });

            return _hudController;
        }
    }


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
    public PlayerFishing Fishing => _fishing;
    public PlayerDialogue Dialogue { get; private set; }


    public PlayerInventoryController Inventory { get; private set; }

    #endregion

    private void Awake()
    {
        MoveStrategy = Bind<PlayerMove>();
        VisualStrategy = Bind<PlayerVisual>();
        Translator = Bind<PlayerStateTranslator>();
        Interactor = Bind<PlayerInteracter>();
        Coordinate = Bind<PlayerCoordinate>();
        Dialogue = Bind<PlayerDialogue>();

        Inventory = new PlayerInventoryController(
            new GridInventoryModel(new Vector2Int(10, 3)),
            _mainInventoryView,
            _quickInventoryView,
            _pannelController
        );
        Fishing.Init(this);

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
        foreach (var item in _testItems)
        {
            Inventory.Model.PushItem(item.Item, item.Count);
        }
    }

    private T Bind<T>() where T : MonoBehaviour, IPlayerStrategy
    {
        var obj = gameObject.AddComponent<T>();
        obj.Init(this);

        return obj;
    }
}