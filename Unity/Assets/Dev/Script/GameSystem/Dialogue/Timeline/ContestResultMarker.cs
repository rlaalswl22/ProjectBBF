using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DS.Core;
using DS.Runtime;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ContestResultMarker : Marker, INotification
{
    public PropertyName id => "ContestResult";

    [SerializeField] private int _chapter;
    [SerializeField] private TimelineAsset _timelineAsset;

    public int Chapter => _chapter;
    public TimelineAsset TimelineAsset => _timelineAsset;
}