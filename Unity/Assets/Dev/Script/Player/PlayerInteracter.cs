using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using ProjectBBF.Event;
using UnityEngine;


public class PlayerInteracter : MonoBehaviour, IPlayerStrategy
{
    private PlayerController _controller;

    public void Init(PlayerController controller)
    {
        _controller = controller;
    }

    public async UniTask OnEnter()
    {
        await UniTask.Delay(100, DelayType.DeltaTime, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);

        Interact();

        await UniTask.Delay(100, DelayType.DeltaTime, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
    }

    private void Interact()
    {
        var interaction = FindCloserObject();

        if (interaction is null) return;
        
        CollisionInteractionUtil
            .CreateSelectState()
            .Bind<IBODestoryTile>(DestroyTile)
            .Bind<IBOCultivateTile>(CultivateTile)
            .Bind<IBOPlantTile>(PlantTile)
            .Execute(interaction.ContractInfo);
    }

    public bool PlantTile(IBOPlantTile action)
    {
        var targetPos = _controller.Coordinate.GetFront();
        ItemData data = _controller.QuickInventory.CurrentItemData;
        IInventorySlot slot = _controller.QuickInventory.CurrentItemSlot;
        
        bool success = false;
        
        if (data is GrownItemData grownData && action.Plant(targetPos, grownData.Definition))
        {
            success = slot.TrySetCount(slot.Count - 1, true);
        }
        
        _controller.QuickInventory.ViewRefresh();
        _controller.Inventory.ViewRefresh();
        
        return success;
    }

    public bool CultivateTile(IBOCultivateTile action)
    {
        var targetPos = _controller.Coordinate.GetFront();
        ItemData data = _controller.QuickInventory.CurrentItemData;
        IInventorySlot slot = _controller.QuickInventory.CurrentItemSlot;


        bool success = false;
        
        if (data.Info.Contains(ToolType.Hoe))
        {
            success = action.TryCultivateTile(targetPos, null);
        }
        
        _controller.QuickInventory.ViewRefresh();
        _controller.Inventory.ViewRefresh();
        
        return success;
    }

    public bool DestroyTile(IBODestoryTile destoryTile)
    {
        var targetPos = _controller.Coordinate.GetFront();
        var data = _controller.QuickInventory.CurrentItemData;

        if (data is null) return false;
        var list = destoryTile.Destory(targetPos, data.Info);

        if (list is null) return false;

        list.ForEach(item =>
        {
            // 아이템 획득에 실패하면 필드에 아이템 드랍
            if (_controller.Inventory.PushItem(item, 1) == false)
            {
                FieldItem.Create(new FieldItem.FieldItemInitParameter()
                {
                    ItemData = item,
                    Position = _controller.transform.position
                });
            }
        });

        _controller.Inventory.ViewRefresh();
        _controller.QuickInventory.ViewRefresh();
        return true;
    }

    public CollisionInteractionMono FindCloserObject()
    {
        var targetPos = _controller.Coordinate.GetFront();
        var colliders = Physics2D.OverlapCircleAll(targetPos, 0.15f);

        float minDis = Mathf.Infinity;
        CollisionInteractionMono minInteraction = null;
        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out CollisionInteractionMono interaction) &&
                interaction.ContractInfo is ObjectContractInfo info
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