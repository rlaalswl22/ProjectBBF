using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Actor/ActorData", fileName = "NewActorData")]
public class ActorData : ScriptableObject
{
    [SerializeField] private string _actorKey;
    [SerializeField] private string _actorName;
    [SerializeField] private string _defaultPortraitKey;
    
    [SerializeField] private FavorabilityEvent _favorabilityEvent;
    [SerializeField] private PortraitTable _portraitTable;

    public string ActorKey => _actorKey;

    public string ActorName => _actorName;

    public string DefaultPortraitKey => _defaultPortraitKey;

    public FavorabilityEvent FavorabilityEvent => _favorabilityEvent;
    public PortraitTable PortraitTable => _portraitTable;
}