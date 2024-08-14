using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActorStrategy
{
}

public interface IBAMove : IActorBehaviour
{
    public bool MoveLock { get; set; }
}
public interface IBAFavorablity : IActorBehaviour
{
    public FavorablityContainer FavorablityContainer { get; }
}

public interface IBANameKey : IActorBehaviour
{
    public string ActorKey { get; }
}