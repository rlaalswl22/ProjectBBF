


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Animal : ActorProxy
{
    [SerializeField] private ActorFavorablity _favorablity;
    [SerializeField] private CollectingMovedActor _collectingMovedActor;
    [SerializeField] private AnimalAnimator _animalAnimator;

    [SerializeField] private List<RuntimeAnimatorController> _collectedAnimations; 
    [SerializeField] private List<RuntimeAnimatorController> _defaultAnimations; 

    protected override void OnInit()
    {
        _favorablity.Init(Owner);
        _collectingMovedActor.Init(Owner);
        _animalAnimator.Init(Owner);

        var collect = _collectingMovedActor.CreateCollectProxyOrNull();

        ContractInfo.AddBehaivour<IBADialogue>(_favorablity);
        if (collect is IBACollect)
        {
            ContractInfo.AddBehaivour<IBACollect>(collect);
        }
        else if (collect is IBACollectTool)
        {
            ContractInfo.AddBehaivour<IBACollectTool>(collect);
        }
        
        
        _collectingMovedActor.OnChangedCollectingState.AddListener(OnChangedCollectingState);
    }

    protected override void OnDoDestroy()
    {
        _collectingMovedActor.OnChangedCollectingState.RemoveListener(OnChangedCollectingState);
    }


    private void OnChangedCollectingState(CollectState state)
    {
        if (state == CollectState.Collected)
        {
            SetRandomAnimator(_collectedAnimations);
        }
        else
        {
            SetRandomAnimator(_defaultAnimations);
        }
    }

    private void SetRandomAnimator(List<RuntimeAnimatorController> anis)
    {
        if (anis.Count == 0) return;
        int index = Random.Range(0, anis.Count - 1);

        Owner.Visual.RuntimeAnimator = anis[index];
    }
}