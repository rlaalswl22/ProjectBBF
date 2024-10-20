using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

namespace ProjectBBF.Event
{
    public struct IntEvent : IEvent
    {
        public int Value;
    }

    [CreateAssetMenu(menuName = "ProjectBBF/Event/Primitive/Int", fileName = "New int event")]
    public class ESOInt : ESOGeneric<IntEvent>
    {
    }
}