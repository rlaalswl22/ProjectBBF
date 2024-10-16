using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

public struct StringEvent : IEvent
{
    public string Value;
}

[CreateAssetMenu(menuName = "ProjectBBF/Event/String", fileName = "New String event")]
public class ESOString : ESOGeneric<StringEvent>
{
}