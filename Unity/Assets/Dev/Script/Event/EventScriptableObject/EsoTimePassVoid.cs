using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Event/Time/EsoTimePassVoid", fileName = "New EsoTimePassVoid")]
public class EsoTimePassVoid : ESOVoid
{
    [SerializeField] private GameTime _toPassTime;

    public GameTime ToPassTime => _toPassTime;
    
}