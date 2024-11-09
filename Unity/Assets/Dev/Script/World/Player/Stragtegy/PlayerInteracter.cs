using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MyBox;
using ProjectBBF.Event;
using ProjectBBF.Persistence;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously


public class PlayerInteracter : MonoBehaviour, IPlayerStrategy
{
    /** 컨트롤 필드 */
    private PlayerController _controller;

    private ActorVisual _visual;
    private PlayerBlackboard _blackboard;
    private PlayerMove _move;
    private PlayerCoordinate _coordinate;
    private SpriteRenderer _indicator;
    private SpriteRenderer _itemPreviewRenderer;
    private bool _isAnyUIVisible;

    /** Interaction 필드 */
    private List<CollisionInteractionMono> _closerObjects = new(5);

    public CollisionInteractionMono CloserObject { get; private set; }
    public event Action<CollisionInteractionMono> OnChangedCloserObject;
    public Vector2 IndicatedPosition => _indicator.transform.position;

    public void Init(PlayerController controller)
    {
        _controller = controller;
        _visual = controller.VisualStrategy;
        _move = controller.MoveStrategy;
        _coordinate = controller.Coordinate;
        _indicator = controller.InteractorIndicator;
        _indicator.enabled = false;
        _itemPreviewRenderer = controller.ItemPreviewRenderer;
        _blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");

        ItemPreviewSprite = null;
    }

    #region Properties

    public bool MainInventoryVisible
    {
        get => _controller.Inventory.MainInvVisible;
        set => _controller.Inventory.MainInvVisible = value;
    }

    public bool QuickInventoryVisible
    {
        get => _controller.Inventory.QuickInvVisible;
        set => _controller.Inventory.QuickInvVisible = value;
    }

    public Sprite ItemPreviewSprite
    {
        get => _itemPreviewRenderer.sprite;
        set
        {
            _itemPreviewRenderer.enabled = value;
            _itemPreviewRenderer.sprite = value;
        }
    }

    #endregion

    #region Wait Methods

    public async UniTask WaitForSecondAsync(float sec, CancellationToken token = default)
    {
        token = CancellationTokenSource.CreateLinkedTokenSource(token, this.GetCancellationTokenOnDestroy()).Token;

        _move.ResetVelocity();
        _blackboard.IsInteractionStopped = true;
        _blackboard.IsMoveStopped = true;

        _ = await UniTask
            .Delay((int)(sec * 1000f), DelayType.DeltaTime, PlayerLoopTiming.Update, token)
            .SuppressCancellationThrow();

        _blackboard.IsInteractionStopped = false;
        _blackboard.IsMoveStopped = false;
    }

    public void WaitForSecond(float sec)
    {
        _ = WaitForSecondAsync(sec);
    }

    public void WaitForDefault()
    {
        WaitForSecond(0.3f);
    }

    public void WaitForPickupAnimation(Vector2 dir)
    {
        _visual.LookAt(dir, AnimationActorKey.Action.Collect, true);
        WaitForSecond(0.3f);
    }

    #endregion

    #region Public Method

    public void AddCloserObject(CollisionInteractionMono interaction)
    {
        _closerObjects.Add(interaction);
    }

    public void RemoveCloserObject(CollisionInteractionMono interaction)
    {
        _closerObjects.Remove(interaction);
    }

    public bool ContainsCloserObject(CollisionInteractionMono interaction)
    {
        return _closerObjects.Contains(interaction);
    }

    #endregion

    #region Private Method

    private void CalcultateIndicatorPosition()
    {
        ItemData currentData = _controller.Inventory.CurrentItemData;

        if (currentData && (
                currentData.Info.Contains(ToolType.Hoe) ||
                currentData.Info.Contains(ToolType.WaterSpray) ||
                currentData.Info.Contains(ToolType.Fertilizer) ||
                currentData.Info.Contains(ToolType.Seed) ||
                currentData.Info.Contains(ToolType.Pickaxe)
            ))
        {
            var obj = FindCloserObject();
            if (obj == false) return;


            Vector2 clickPoint = Camera.main.ScreenToWorldPoint(InputManager.Map.Player.Look.ReadValue<Vector2>());
            Vector2 dir = clickPoint - (Vector2)_controller.transform.position;
            var pos =
                    (Vector2)_controller.transform.position +
                    (Vector2)_coordinate.GetDirOffset(_visual.CalculateLookDir(dir, true))
                ;

            if (obj.TryGetContractInfo(out ObjectContractInfo info) &&
                info.TryGetBehaviour(out IBOInteractIndicator interactIndicator) &&
                interactIndicator.CanDraw(pos))
            {
                _indicator.enabled = true;
                _indicator.transform.position = interactIndicator.GetDrawPositionAndSize(pos).position;
                return;
            }
        }

        _indicator.enabled = false;
    }

    private void CalculateCloseObject()
    {
        float minDis = Mathf.Infinity;
        CollisionInteractionMono minObj = null;

        int nullCount = 0;

        foreach (CollisionInteractionMono obj in _closerObjects)
        {
            if (obj == false)
            {
                nullCount++;
                continue;
            }

            float dis = ((Vector2)(obj.transform.position - _controller.transform.position)).sqrMagnitude;

            if (dis < minDis)
            {
                minDis = dis;
                minObj = obj;
            }
        }

        if (CloserObject != minObj)
        {
            CloserObject = minObj;
            OnChangedCloserObject?.Invoke(CloserObject);
        }

        if (nullCount >= 10)
        {
            _closerObjects.RemoveAll(x => x == false);
        }
    }

    private void PlayAudio(ItemData itemData, string usingKey)
    {
        if (itemData == false) return;

        if (itemData.UseActionUsingActionAudioInfos is null) return;

        foreach (var info in itemData.UseActionUsingActionAudioInfos)
        {
            if (info.HasAudio(usingKey))
            {
                AudioManager.Instance.PlayOneShot(info.MixerGroupKey, info.AudioKey);
            }
        }
    }

    public CollisionInteractionMono FindCloserObject()
    {
        var targetPos = _controller.Coordinate.GetFront();
        var colliders =
            Physics2D.OverlapCircleAll(targetPos, _controller.CoordinateData.Radius,
                ~LayerMask.GetMask("Player", "Ignore Raycast"));

        float minDis = Mathf.Infinity;
        CollisionInteractionMono minInteraction = null;
        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out CollisionInteractionMono interaction)
               )
            {
                float dis = (transform.position - col.transform.position).sqrMagnitude;
                if (dis < minDis)
                {
                    minInteraction = interaction;
                    minDis = dis;
                }
            }
        }

        return minInteraction;
    }

    #endregion

    #region Callback Method
    
    public async UniTask<bool> OnToolAction()
    {
        if (_blackboard.IsInteractionStopped) return false;

        try
        {
            ItemData data = _controller.Inventory.CurrentItemData;
            if (data == false) return false;

            foreach (PlayerItemBehaviour behaviour in data.PlayerItemBehaviours)
            {
                await behaviour.DoAction(_controller, data, this.GetCancellationTokenOnDestroy());
            }
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            Debug.LogException(e);
        }
        finally
        {
        }
        
        return false;
    }

    private void OnInteractObject(IBOInteractive obj) => obj.UpdateInteract(_controller.Interaction);

    private bool Farmland(CollisionInteractionMono interaction)
    {
        CollisionInteractionUtil
            .CreateSelectState()
            .Bind<IBODestoryTile>(DestroyTile)
            .Bind<IBOFertilizerTile>(FertilizerTile)
            .Bind<IBOSprinkleWaterTile>(SprinkleWater)
            .Bind<IBOCultivateTile>(CultivateTile)
            .Bind<IBOPlantTile>(PlantTile)
            .Execute(interaction.ContractInfo, out bool executedAny);

        return executedAny;
    }

    private bool ToolCollect(CollisionInteractionMono interaction)
    {
        CollisionInteractionUtil
            .CreateSelectState()
            .Bind<IBOCollectPlant>(CollectPlant)
            .Bind<IBOInteractiveTool>(CollectObject)
            .Execute(interaction.ContractInfo, out bool executedAny);

        return executedAny;
    }

    public bool PlantTile(IBOPlantTile action)
    {
        var targetPos = _controller.Coordinate.GetFront();
        ItemData data = _controller.Inventory.CurrentItemData;
        IInventorySlot slot = _controller.Inventory.CurrentItemSlot;

        bool success = false;

        if (data is PlantItemData grownData && action.Plant(targetPos, grownData.Definition))
        {
            success = slot.TryAdd(-1, true) is SlotStatus.Success;
        }

        if (success)
        {
            AudioManager.Instance.PlayOneShot("Player", "Player_Tool_Using_Fertilizer");
        }

        _controller.Inventory.Refresh();

        return success;
    }

    private bool FertilizerTile(IBOFertilizerTile action)
    {
        var targetPos = _controller.Coordinate.GetFront();
        ItemData data = _controller.Inventory.CurrentItemData;
        IInventorySlot slot = _controller.Inventory.CurrentItemSlot;

        if (data is null) return false;
        if (slot is null) return false;

        bool success = false;

        if (data.Info.Contains(ToolType.Fertilizer))
        {
            if (slot.Data is FertilizerItemData fertilizerItem)
            {
                success = action.PlantFertilizer(targetPos, fertilizerItem.TargetTile);

                if (success)
                    slot.TryAdd(-1, true);
            }
        }

        _controller.Inventory.Refresh();

        return success;
    }

    public bool CultivateTile(IBOCultivateTile action)
    {
        var targetPos = _controller.Coordinate.GetFront();
        ItemData data = _controller.Inventory.CurrentItemData;

        if (data is null) return false;

        bool success = false;

        if (data.Info.Contains(ToolType.Hoe))
        {
            success = action.TryCultivateTile(targetPos, null);
        }

        _controller.Inventory.Refresh();

        if (success)
        {
            AudioManager.Instance.PlayOneShot("Player", "Player_Digging");
        }

        return success;
    }

    private bool SprinkleWater(IBOSprinkleWaterTile action)
    {
        var targetPos = _controller.Coordinate.GetFront();
        ItemData data = _controller.Inventory.CurrentItemData;

        if (data is null) return false;

        bool success = false;

        if (data.Info.Contains(ToolType.WaterSpray))
        {
            success = action.SprinkleWater(targetPos);
        }

        return success;
    }

    public bool DestroyTile(IBODestoryTile destoryTile)
    {
        var targetPos = _controller.Coordinate.GetFront();
        var data = _controller.Inventory.CurrentItemData;

        if (data is null) return false;
        var list = destoryTile.Destory(targetPos, data.Info);

        if (list is null) return false;

        list.ForEach(item =>
        {
            // 아이템 획득에 실패하면 필드에 아이템 드랍
            if (_controller.Inventory.Model.PushItem(item, 1) is not 0)
            {
                // TODO: 레거시
                //FieldItem.Create(new FieldItem.FieldItemInitParameter()
                //{
                //    ItemData = item,
                //    Position = _controller.transform.position
                //});
            }
        });

        if (list.Any())
        {
            AudioManager.Instance.PlayOneShot("Player", "Player_Harvest");
        }

        _controller.Inventory.Refresh();
        return true;
    }

    private bool CollectPlant(IBOCollectPlant action)
    {
        var targetPos = _controller.Coordinate.GetFront();
        var data = _controller.Inventory.CurrentItemData;
        
        WaitForDefault();
        _visual.LookAt(targetPos - _controller.transform.position, AnimationActorKey.Action.Plant, true);

        List<ItemData> items = new List<ItemData>(2);

        if (action.Collect(targetPos, items) is false) return false;


        AudioManager.Instance.PlayOneShot("Player", "Player_Harvest");

        foreach (var item in items)
        {
            _controller.Inventory.Model.PushItem(item, 1);
        }

        _controller.Inventory.Refresh();

        return true;
    }

    private bool CollectObject(IBOInteractiveTool action)
    {
        var itemData = _controller.Inventory.CurrentItemData;
        if (itemData == false) return false;

        foreach (ToolRequireSet set in itemData.Info.Sets)
        {
            if (action.IsVaildTool(set))
            {
                action.UpdateInteract(_controller.Interaction);
                return true;
            }
        }


        return false;
    }

    private bool IsTriggeredUIAny =>
        InputManager.Map.UI.Inventory.triggered ||
        InputManager.Map.UI.Setting.triggered ||
        InputManager.Map.UI.RecipeBook.triggered ||
        InputManager.Map.UI.CloseUI.triggered;

    private async UniTask OnUIInventory()
    {
        _ = await UniTask
            .NextFrame(PlayerLoopTiming.PostLateUpdate, this.GetCancellationTokenOnDestroy())
            .SuppressCancellationThrow();

        _controller.PannelView.ViewState = PlayerPannelView.ViewType.Inv;
        _controller.Blackboard.IsMoveStopped = true;
        _controller.Blackboard.IsInteractionStopped = true;
        _controller.QuestPresenter.Visible = true;
        _isAnyUIVisible = true;

        _ = await UniTask
            .WaitUntil(() => IsTriggeredUIAny, PlayerLoopTiming.PostLateUpdate,
                this.GetCancellationTokenOnDestroy())
            .SuppressCancellationThrow();

        _controller.PannelView.ViewState = PlayerPannelView.ViewType.Close;
        _controller.Blackboard.IsMoveStopped = false;
        _controller.Blackboard.IsInteractionStopped = false;
        _isAnyUIVisible = false;
    }

    private async UniTask OnUISetting()
    {
        _ = await UniTask
            .NextFrame(PlayerLoopTiming.PostLateUpdate, this.GetCancellationTokenOnDestroy())
            .SuppressCancellationThrow();

        _controller.PannelView.ViewState = PlayerPannelView.ViewType.Setting;
        _controller.Blackboard.IsMoveStopped = true;
        _controller.Blackboard.IsInteractionStopped = true;
        _controller.RecipeBookPresenter.PreviewSummaryView.Visible = false;
        _controller.QuestPresenter.Visible = false;
        _isAnyUIVisible = true;

        _ = await UniTask
            .WaitUntil(() => IsTriggeredUIAny, PlayerLoopTiming.PostLateUpdate,
                this.GetCancellationTokenOnDestroy())
            .SuppressCancellationThrow();

        _controller.PannelView.ViewState = PlayerPannelView.ViewType.Close;
        _controller.Blackboard.IsMoveStopped = false;
        _controller.Blackboard.IsInteractionStopped = false;
        if (_controller.RecipeBookPresenter.PreviewSummaryView.Data is not null)
            _controller.RecipeBookPresenter.PreviewSummaryView.Visible = true;
        _controller.QuestPresenter.Visible = true;
        _isAnyUIVisible = false;
    }

    private async UniTask OnUIRecipe()
    {
        _ = await UniTask
            .NextFrame(PlayerLoopTiming.PostLateUpdate, this.GetCancellationTokenOnDestroy())
            .SuppressCancellationThrow();

        _controller.PannelView.ViewState = PlayerPannelView.ViewType.Close;
        _controller.Blackboard.IsMoveStopped = true;
        _controller.Blackboard.IsInteractionStopped = true;
        _controller.RecipeBookPresenter.ListView.Visible = true;
        _controller.RecipeBookPresenter.PreviewView.Visible = true;
        _isAnyUIVisible = true;

        _ = await UniTask
            .WaitUntil(() => IsTriggeredUIAny, PlayerLoopTiming.PostLateUpdate,
                this.GetCancellationTokenOnDestroy())
            .SuppressCancellationThrow();

        _controller.Blackboard.IsMoveStopped = false;
        _controller.Blackboard.IsInteractionStopped = false;
        _controller.RecipeBookPresenter.ListView.Visible = false;
        _controller.RecipeBookPresenter.PreviewView.Visible = false;
        _isAnyUIVisible = false;
    }

    #endregion

    #region Unity Method

    private void Update()
    {
        CalculateCloseObject();
        CalcultateIndicatorPosition();

        if (_blackboard.IsInteractionStopped) return;

        if (CloserObject)
        {
            CollisionInteractionUtil
                .CreateState()
                .Bind<IBOInteractive>(OnInteractObject)
                .Execute(CloserObject.ContractInfo);
        }

        if (_isAnyUIVisible is false)
        {
            if (InputManager.Map.UI.Inventory.triggered)
            {
                _ = OnUIInventory();
            }

            if (InputManager.Map.UI.Setting.triggered)
            {
                _ = OnUISetting();
            }

            if (InputManager.Map.UI.RecipeBook.triggered)
            {
                _ = OnUIRecipe();
            }
        }

        if (InputManager.Map.Player.UseTool.triggered)
        {
            _ = OnToolAction();
        }
    }

    #endregion
}