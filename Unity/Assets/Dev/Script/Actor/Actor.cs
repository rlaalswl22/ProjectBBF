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

    [SerializeField] private PatrolPointPath _patrolPath;

    [field: SerializeField] private AnimationData _aniData;
    
    [field: SerializeField, AutoProperty, MustBeAssigned, InitializationField]
    private CollisionInteraction _interaction;

    [SerializeField] private float speed;

    [SerializeField] private StateTransitionHandler _transitionHandler;
    
    
    private FavorablityContainer _favorablityContainer;

    public FavorablityContainer FavorablityContainer => _favorablityContainer;

    public PatrolPointPath PatrolPath => _patrolPath;

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

        _transitionHandler.AddHandleCallback("DailyRoutine", ToDailyRoutine);
        _transitionHandler.AddHandleCallback("TalkingForPlayer", ToTalkingForPlayer);
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
    
    public async UniTask<Vector2> MoveToPoint(Vector2 pos)
    {
        await UniTask.WaitUntil(() =>
        {
            transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * speed);

            return Vector2.Distance(transform.position, pos) < 0.001f;
        }, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);

        return pos;
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


    #region StatehandleCallback
    private bool ToDailyRoutine()
    {
        return false;
    }
    private bool ToTalkingForPlayer()
    {
        return false;
    }

    #endregion

}
