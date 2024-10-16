using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DS.Core;
using DS.Runtime;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MoveToSceneMarker : Marker, INotification
{
    public PropertyName id => "MoveToScene";

    [SerializeField] private ESOVoid _eso;

    public ESOVoid ESO => _eso;
}