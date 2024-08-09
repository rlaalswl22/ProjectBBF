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
