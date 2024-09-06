using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DS.Core;
using JetBrains.Annotations;
using ProjectBBF.Event;
using ProjectBBF.Persistence;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerInteracter : MonoBehaviour, IPlayerStrategy
{
    private PlayerController _controller;
    private PlayerBlackboard _blackboard;
    private PlayerMove _move;

    public void Init(PlayerController controller)
    {
        _controller = controller;
        _move = controller.MoveStrategy;
        _blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");
    }

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
    
    public async UniTask<bool> OnToolAction()
    {
        try
        {
            ItemData currentData = _controller.Inventory.CurrentItemData;

            if (currentData && (
                    currentData.Info.Contains(ToolType.Hoe) ||
                    currentData.Info.Contains(ToolType.WaterSpray) ||
                    currentData.Info.Contains(ToolType.Fertilizer) ||
                    currentData.Info.Contains(ToolType.Seed) 
                ))
            {
                if (_blackboard.Energy < 1)
                {
                    return false;
                }
                
                _move.ResetVelocity();
                _blackboard.Energy--;
            }
            else
            {
                return false;
            }

            var interaction = FindCloserObject();
            if (interaction is null)
            {
                await UniTask.Delay(300, DelayType.DeltaTime, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
                return false;
            }

            if (Farmland(interaction) is false)
            {
                await UniTask.Delay(300, DelayType.DeltaTime, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
                return false;
            }
            
            await UniTask.Delay(300, DelayType.DeltaTime, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());

        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            Debug.LogException(e);
            return false;
        }
        
        return true;
    }

    public async UniTask<bool> OnCollectAction()
    {
        try
        {
            var interaction = FindCloserObject();
            if (interaction is null) return false;

            CollisionInteractionUtil
                .CreateSelectState()
                .Bind<IBOCollectPlant>(CollectPlant)
                .Bind<IBOCollect>(CollectObject)
                .Bind<IBACollect>(CollectObject)
                .Execute(interaction.ContractInfo, out bool executedAny);

            if (executedAny)
            {
                _move.ResetVelocity();
                await UniTask.Delay(300, DelayType.DeltaTime, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
            }
            
            return executedAny;
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            Debug.LogException(e);
            return false;
        }

        return false;
    }


    public async UniTask<bool> OnDialogueAction()
    {
        try
        {
            var interaction = FindCloserObject();
            if (interaction is null) return false;

            _move.ResetVelocity();
            
            bool success = await Dialogue(interaction);

            DialogueController.Instance.ResetDialogue();
            _controller.Inventory.QuickInvVisible = true;

            if (success)
            {
                return true;
            }
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            Debug.LogException(e);
        }

        return false;
    }

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

    private async UniTask<bool> Dialogue(CollisionInteractionMono interaction)
    {
        if (interaction.TryGetContractInfo(out ActorContractInfo actorInfo) &&
            actorInfo.TryGetBehaviour(out IBAStateTransfer stateTransfer) &&
            actorInfo.TryGetBehaviour(out IBAFavorablity favorablity) &&
            actorInfo.TryGetBehaviour(out IBANameKey nameKey))
        {
            var favorablityContainer = favorablity.FavorablityContainer;

            //TODO: test code, delete this
            favorablityContainer.CurrentFavorablity = 3;

            var eventItems = favorablityContainer.GetExecutableDialogues();

            if (eventItems.Count == 0)
            {
                stateTransfer.TranslateState("DailyRoutine");
                return false;
            }

            stateTransfer.TranslateState("TalkingForPlayer");
            _controller.Inventory.HideAll();

            if (actorInfo.Interaction.Owner is Actor actor)
            {
                actor.Visual.LookAt(_controller.transform.position - actor.transform.position,
                    AnimationData.Movement.Idle);
            }
            
            var instance = DialogueController.Instance;
            instance.ResetDialogue();
            instance.Visible = true;
            instance.SetDisplayName(nameKey.ActorKey);

            if (ActorDataManager.Instance.CachedDict.TryGetValue(nameKey.ActorKey, out var data))
            {
                instance.SetPortrait(data.ActorKey, data.DefaultPortraitKey);
            }

            var index = await instance.GetBranchResultAsync(
                "대화",
                "선물",
                "떠나기"
            );

            switch (index)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    instance.ResetDialogue();
                    stateTransfer.TranslateState("DailyRoutine");
                    return false;
                default:
                    instance.ResetDialogue();
                    stateTransfer.TranslateState("DailyRoutine");
                    return false;
            }


            // 한번만 실행해야하는 이벤트는 블랙리스트에 등록
            foreach (FavorabilityEventItem item in eventItems)
            {
                if (item.Once)
                {
                    favorablityContainer.AddExecutedDialogueGuid(item.Container.Guid);
                }

                DialogueContext context = instance.CreateContext(item.Container);

                await UniTask.Yield();
                await context.Next();

                while (context.CanNext)
                {
                    await UniTask.Yield();
                    if (InputManager.Actions.DialogueSkip.triggered)
                    {
                        await UniTask.Yield();
                        await context.Next();
                    }
                }

                await UniTask.WaitUntil(() => InputManager.Actions.DialogueSkip.triggered, PlayerLoopTiming.Update,
                    GlobalCancelation.PlayMode);
            }


            stateTransfer.TranslateState("DailyRoutine");
            return true;
        }
        
        
        return false;
    }

    #region Actor

    #endregion

    #region Object

    public bool PlantTile(IBOPlantTile action)
    {
        print("PlantTile");
        var targetPos = _controller.Coordinate.GetFront();
        ItemData data = _controller.Inventory.CurrentItemData;
        IInventorySlot slot = _controller.Inventory.CurrentItemSlot;

        bool success = false;

        if (data is PlantItemData grownData && action.Plant(targetPos, grownData.Definition))
        {
            success = slot.TryAdd(-1, true) is SlotStatus.Success;
        }

        _controller.Inventory.Refresh();

        return success;
    }

    private bool FertilizerTile(IBOFertilizerTile action)
    {
        print("FertilizerTile");
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
        print("CultivateTile");
        var targetPos = _controller.Coordinate.GetFront();
        ItemData data = _controller.Inventory.CurrentItemData;

        if (data is null) return false;

        bool success = false;

        if (data.Info.Contains(ToolType.Hoe))
        {
            success = action.TryCultivateTile(targetPos, null);
        }

        _controller.Inventory.Refresh();

        return success;
    }

    private bool SprinkleWater(IBOSprinkleWaterTile action)
    {
        print("SprinkleWater");
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
        print("DestroyTile");
        var targetPos = _controller.Coordinate.GetFront();
        var data = _controller.Inventory.CurrentItemData;

        if (data is null) return false;
        var list = destoryTile.Destory(targetPos, data.Info);

        if (list is null) return false;

        list.ForEach(item =>
        {
            // 아이템 획득에 실패하면 필드에 아이템 드랍
            if (_controller.Inventory.Model.PushItem(item, 1) == false)
            {
                FieldItem.Create(new FieldItem.FieldItemInitParameter()
                {
                    ItemData = item,
                    Position = _controller.transform.position
                });
            }
        });

        _controller.Inventory.Refresh();
        return true;
    }

    private bool CollectPlant(IBOCollectPlant action)
    {
        print("CollectPlant");
        var targetPos = _controller.Coordinate.GetFront();
        var data = _controller.Inventory.CurrentItemData;

        List<ItemData> items = new List<ItemData>(2);

        if (action.Collect(targetPos, items) is false) return false;

        foreach (var item in items)
        {
            _controller.Inventory.Model.PushItem(item, 1);
        }

        _controller.Inventory.Refresh();

        return true;
    }

    private bool CollectObject(IBOCollect action)
    {
        var list = action.Collect();
        if (list is null) return false;

        if (action.Interaction.Owner is Actor actor)
        {
            actor.Visual.LookAt(transform.position - actor.transform.position, AnimationData.Movement.Idle);
            actor.TransitionHandler.TranslateState("ToWait");
        }

        ColectObject(list);

        return true;
    }

    private bool CollectObject(IBACollect action)
    {
        var list = action.Collect();
        if (list is null) return false;


        if (action.Interaction.Owner is Actor actor)
        {
            actor.Visual.LookAt(transform.position - actor.transform.position, AnimationData.Movement.Idle);
            actor.TransitionHandler.TranslateState("ToWait");
        }
        
        ColectObject(list);

        return true;
    }

    private void ColectObject(List<ItemData> items)
    {
        if (items is null) return;

        foreach (ItemData item in items)
        {
            _controller.Inventory.Model.PushItem(item, 1);
        }

        _controller.Inventory.Refresh();
    }

    #endregion

    public CollisionInteractionMono FindCloserObject()
    {
        var targetPos = _controller.Coordinate.GetFront();
        var colliders =
            Physics2D.OverlapCircleAll(targetPos, _controller.InteractionRadius, ~LayerMask.GetMask("Player"));

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
}