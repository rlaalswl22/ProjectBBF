using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Behaviour/Item/FishingRod", fileName = "Bav_Item_FishingRod")]
public class PIBFishingRod : PlayerItemBehaviour
{
    public override async UniTask DoAction(PlayerController playerController, ItemData itemData, CancellationToken token = default)
    {
        if (itemData && itemData.Info.Contains(ToolType.FishingRod))
        {
            if (playerController.Fishing.IsFishing is false)
            {
                await playerController.Fishing.Fishing();
            }
        }

    }
}