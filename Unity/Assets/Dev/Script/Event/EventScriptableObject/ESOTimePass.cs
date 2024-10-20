using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

public struct TimePassEvent : IEvent
{
    public GameTime ToPassTime;
}

[CreateAssetMenu(menuName = "ProjectBBF/Event/Time/EsoTimePass", fileName = "New EsoTimePass")]
public class ESOTimePass : ESOGeneric<TimePassEvent>
{
}