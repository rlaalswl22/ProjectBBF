using System;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Runtime
{
    [CreateAssetMenu(menuName = "ProjectBBF/Dialogue/Favorability", fileName = "Favorability")]
    public class FavorabilityDescriptor : ExecutionDescriptorT<int>
    {
        protected override void OnExecute(int arg0)
        {
            Debug.Log($"호감도 +{arg0}!");
        }
    }
}