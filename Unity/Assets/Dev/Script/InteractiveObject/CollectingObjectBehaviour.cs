using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

public abstract class CollectingObjectBehaviour : ScriptableObject
{
    public abstract void InitBehaviour(CollectingObjectData data, CollisionInteraction interaction, ObjectContractInfo info);
}
