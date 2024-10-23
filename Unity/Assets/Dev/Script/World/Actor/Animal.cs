


using UnityEngine;

public class Animal : ActorProxy
{
    [SerializeField] private ActorFavorablity _favorablity;
    [SerializeField] private CollectingMovedActor _collectingMovedActor;
    [SerializeField] private AnimalAnimator _animalAnimator;

    protected override void OnInit()
    {
        _favorablity.Init(Owner);
        _collectingMovedActor.Init(Owner);
        _animalAnimator.Init(Owner);

        ContractInfo
            .AddBehaivour<IBADialogue>(_favorablity)
            .AddBehaivourSelect<IBACollect, IBACollectTool>(_collectingMovedActor.CreateCollectProxyOrNull(), true)
            ;
        
        _collectingMovedActor.OnChangedCollectingState.AddListener(OnChangedCollectingState);
    }

    protected override void OnDoDestroy()
    {
        _collectingMovedActor.OnChangedCollectingState.RemoveListener(OnChangedCollectingState);
    }


    private void OnChangedCollectingState(CollectingMovedActor.CollectState state)
    {
        
    }
}