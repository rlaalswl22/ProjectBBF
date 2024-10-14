using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

public struct GiveItemEvent : IEvent
{
    public List<ItemDataSerializedSet> Items;
}

[CreateAssetMenu(menuName = "ProjectBBF/Event/GiveItem", fileName = "New GiveItem")]
public class ESOGiveItem : ESOGeneric<GiveItemEvent>
{
}