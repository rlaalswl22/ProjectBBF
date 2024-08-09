using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBAMove : IActorBehaviour
{
    public void SetMoveLock(bool value);
    public bool GetMoveLock();
}
public interface IBAFavorablity : IActorBehaviour
{
    public FavorablityContainer GetFavorablityContainer();
}

public interface IBANameKey : IActorBehaviour
{
    public string GetActorKey();
}