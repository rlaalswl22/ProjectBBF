using System;
using System.Collections;
using System.Collections.Generic;
using DS.Core;
using UnityEngine;


public abstract class DialogueItem
{
    public string CharacterDisplayName { get; private set; }
    public string PortraitKey { get; private set; }

    protected DialogueItem(string characterDisplayName, string portraitKey)
    {
        CharacterDisplayName = characterDisplayName;
        PortraitKey = portraitKey;
    }
}

public abstract class DialogueItemT<T> : DialogueItem where T : DialogueRuntimeNode
{
    public readonly T Node;
    protected DialogueItemT(T node, string characterDisplayName, string portraitKey)
        : base(characterDisplayName, portraitKey)
    {
        Node = node;
    }
}