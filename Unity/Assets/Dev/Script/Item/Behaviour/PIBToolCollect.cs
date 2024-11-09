using System.Threading;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Behaviour/Item/ToolCollect ",  fileName = "Bav_Item_ToolCollect")]
public class PIBToolCollect : PlayerItemBehaviour
{
    public override async UniTask DoAction(PlayerController playerController, ItemData itemData, CancellationToken token = default)
    {
        await UniTask.CompletedTask;
        
        var interaction = playerController.Interactor.FindCloserObject();
        if (interaction == false) return;

        CollisionInteractionUtil
            .CreateSelectState()
            .Bind<IBOInteractiveTool>(x => CollectObject(x, playerController))
            .Execute(interaction.ContractInfo, out bool _);
    }
    
    private bool CollectObject(IBOInteractiveTool action, PlayerController pc)
    {
        var itemData = pc.Inventory.CurrentItemData;
        if (itemData == false) return false;

        foreach (ToolRequireSet set in itemData.Info.Sets)
        {
            if (action.IsVaildTool(set))
            {
                action.UpdateInteract(pc.Interaction);
                return true;
            }
        }


        return false;
    }
}