using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/InteractiveObject/Behaviour/Burn", fileName = "NewBurn")]
public class BurnBehaviour : CollectingObjectBehaviour, IBOBurn
{
    public CollisionInteraction Interaction { get; private set; }
    private CollectingObjectData _data;
    
    public override void InitBehaviour(CollectingObjectData data, CollisionInteraction interaction, ObjectContractInfo info)
    {
        Interaction = interaction;
        _data = data;
        
        info.AddBehaivour<IBOBurn>(this);
    }

    public async UniTaskVoid DoFire()
    {
        await UniTask.Delay((int)(_data.BuringTime));
    }
}
