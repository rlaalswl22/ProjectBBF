
using ProjectBBF.Event;
using UnityEngine;

public abstract class ActorComFavorability : ActorComponent, IBADialogue
{
    public CollisionInteraction Interaction { get; private set; }

    protected Actor Owner { get; private set; }
    protected FavorabilityData FavorabilityData { get; private set; }

    public override void Init(Actor actor)
    {
        Owner = actor;
        Interaction = actor.Interaction;
        if (ActorDataManager.Instance.CachedDict.TryGetValue(actor.ActorKey, out var favora) is false)
        {
            Debug.LogError("유효하지 않은 ActorKey");
            return;
        }
        
        FavorabilityData = favora;

        if (Interaction.ContractInfo is ActorContractInfo info)
        {
            info.AddBehaivour<IBADialogue>(this);
        }
        else
        {
            Debug.LogError("유효하지 않은 ContractInfo");
        }
    }

    public abstract DialogueEvent DequeueDialogueEvent();
    public abstract DialogueEvent PeekDialogueEvent();
}