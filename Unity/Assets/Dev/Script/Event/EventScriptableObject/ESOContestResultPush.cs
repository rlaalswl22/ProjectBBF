using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

public struct ContestResultEvent : IEvent
{
    public ItemData TargetItem;
}

[CreateAssetMenu(menuName = "ProjectBBF/Event/ContestResultPush", fileName = "New ContestResultPush")]
public class ESOContestResultPush : ESOGeneric<ContestResultEvent>
{
}