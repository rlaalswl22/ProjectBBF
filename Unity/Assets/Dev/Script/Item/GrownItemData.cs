using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/GrownItemData", fileName = "NewGrownItemData")]
public class GrownItemData : ItemData
{
    [SerializeField] private GrownDefinition _definition;

    public GrownDefinition Definition => _definition;
}
