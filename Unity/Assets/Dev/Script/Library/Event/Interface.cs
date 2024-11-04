using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

/*
 * Base interaction
 * --
 */
public interface IBaseBehaviour
{
    public CollisionInteraction Interaction { get; }
}
public interface IObjectBehaviour : IBaseBehaviour
{
    
}