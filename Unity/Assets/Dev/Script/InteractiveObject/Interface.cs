using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum EPlayerCollectingAnimation : int
{
    None = 0,
    Hand,
    Shovels
}

public interface IBOBurn : IObjectBehaviour
{
    public UniTaskVoid DoFire();
}
public interface IBOCollect : IObjectBehaviour
{
    public List<ItemData> Collect();
}
public interface IBODestory : IObjectBehaviour
{
    public List<ItemData> Destory();
}