using System;
using UnityEngine;

namespace ProjectBBF.Event
{
    [CreateAssetMenu(menuName = "ProjectBBF/Event/Void", fileName = "New Void")]
    public class ESOVoid : EventScriptableObject
    {
        public event Action OnEventRaised;

        public virtual void Raise()
        {
            OnEventRaised?.Invoke();
        }

        public override void Release()
        {
            OnEventRaised = null;
        }
    }
}