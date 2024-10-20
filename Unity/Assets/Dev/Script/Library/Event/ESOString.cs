using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

namespace ProjectBBF.Event
{
    public struct StringEvent : IEvent
    {
        public string Value;
    }

    [CreateAssetMenu(menuName = "ProjectBBF/Event/Primitive/String", fileName = "New String event")]
    public class ESOString : ESOGeneric<StringEvent>
    {
    }
}