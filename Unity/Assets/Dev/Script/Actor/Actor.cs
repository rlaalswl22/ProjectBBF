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

    [field: SerializeField, MustBeAssigned, InitializationField]
    private FavorabilityEvent _favorabilityEvent;
    
    
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

        _favorablityContainer = new FavorablityContainer(_favorabilityEvent, 0, null);
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
