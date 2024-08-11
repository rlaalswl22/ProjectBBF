using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

    [SerializeField] private Transform pivot1;
    [SerializeField] private Transform pivot2;
    [SerializeField] private float speed;
    
    
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

        CoUpdate().Forget();
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

    private async UniTask CoUpdate()
    {
        while (true)
        {
            ChangeClip(_aniData.MovementUp);
            await UniTask.WaitUntil(() =>
            {
                transform.position = Vector2.MoveTowards(transform.position, pivot1.position, Time.deltaTime * speed);

                return Vector2.Distance(transform.position, pivot1.position) < 0.0001f;
            }, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
            
            
            ChangeClip(_aniData.MovementDown);
            await UniTask.WaitUntil(() =>
                {
                    transform.position = Vector2.MoveTowards(transform.position, pivot2.position, Time.deltaTime * speed);

                    return Vector2.Distance(transform.position, pivot2.position) < 0.0001f;
                }, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
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
