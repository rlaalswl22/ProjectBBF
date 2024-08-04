using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using UnityEngine;

public class PlayerDestroy : MonoBehaviour, IPlayerStrategy
{
    private PlayerController _controller;

    public void Init(PlayerController controller)
    {
        _controller = controller;
    }

    public async UniTask OnEnter()
    {
        await UniTask.Delay(100, DelayType.DeltaTime, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);

        TileDestroy();
        
        await UniTask.Delay(100, DelayType.DeltaTime, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
    }

    public void TileDestroy()
    {
        var interaction = FindCloserObject();

        if (interaction is null) return;

        CollisionInteractionUtil
            .CreateState()
            .Bind<IBODestoryTile>(x =>
            {
                var targetPos = _controller.Coordinate.GetFront();
                var list = x.Destory(targetPos);

                list.ForEach(item => print(item.ItemName));
            })
            .Bind<IBODestory>(x =>
            {
                var list = x.Destory();
                list.ForEach(item => print(item.ItemName));
            })
            .Execute(interaction.ContractInfo);
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