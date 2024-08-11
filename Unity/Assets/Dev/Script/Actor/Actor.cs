using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;

public class Actor : MonoBehaviour, IBAMove, IBAFavorablity, IBANameKey
{
    [field: SerializeField, MustBeAssigned, InitializationField]
    private string _actorKey;

    [field: SerializeField] private AnimationData _aniData;
    
    [field: SerializeField, AutoProperty, MustBeAssigned, InitializationField]
    private CollisionInteraction _interaction;
    
    
    private FavorablityContainer _favorablityContainer;

    public FavorablityContainer FavorablityContainer => _favorablityContainer;

    public CollisionInteraction Interaction => _interaction;
    private void Awake()
    {
        var info = ActorContractInfo.Create(() => gameObject);
        info.AddBehaivour<IBAMove>(this);
        info.AddBehaivour<IBAFavorablity>(this);
        info.AddBehaivour<IBANameKey>(this);
        
        _interaction.SetContractInfo(info, this);

        if (ActorDataManager.Instance.Table.Table.TryGetValue(_actorKey, out var data))
        {
            _favorablityContainer = new FavorablityContainer(data.FavorabilityEvent, 0, null);
        }
        else
        {
            Debug.LogError($"actor key({_actorKey})를 찾을 수 없음");
        }
        
        RuntimeAnimatorController ac = _animator.runtimeAnimatorController;
        AnimatorOverrideController overrideController = new AnimatorOverrideController(ac);
        _animator.runtimeAnimatorController = overrideController;

        _defaultClip = overrideController[PLAYER_ANI_STATE];
        
        ChangeClip(_aniData.IdleDown, true);
    }
    
    [SerializeField]
    private Animator _animator;

    private const string PLAYER_ANI_STATE = "DefaultMovement";

    private AnimationClip _defaultClip = null;
    private AnimationClip _beforeClip = null;
    public void ChangeClip(AnimationClip newClip, bool force = false)
    {
        if (force == false && _beforeClip == newClip) return;
        
        if (_animator.runtimeAnimatorController is AnimatorOverrideController overrideController)
        {
            overrideController[_defaultClip] = newClip;
            _beforeClip = newClip;
        }
    }

    public void SetMoveLock(bool value)
    {
        throw new NotImplementedException();
    }

    public bool GetMoveLock()
    {
        throw new NotImplementedException();
    }

    public FavorablityContainer GetFavorablityContainer()
        => FavorablityContainer;

    public string GetActorKey() 
        => _actorKey;

}
