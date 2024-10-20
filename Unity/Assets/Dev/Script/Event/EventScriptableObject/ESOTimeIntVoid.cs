using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Event/TimeIntVoid", fileName = "New TimeIntVoid")]
public class ESOTimeIntVoid : ESOVoid
{
    [SerializeField] private int _value;

    public int Value => _value;
}