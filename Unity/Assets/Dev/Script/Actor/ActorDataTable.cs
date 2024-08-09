using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ProjectBBF/Actor/ActorDataTable", fileName = "NewActorDataTable")]
public class ActorDataTable : ScriptableObject
{
    
    [SerializeField] private List<ActorData> _datas = new();

    private Dictionary<string, ActorData> _table;

    public Dictionary<string, ActorData> Table
    {
        get
        {
            if (_table is null)
            {
                _table = new Dictionary<string, ActorData>();
                
                _datas.ForEach(x=>_table.Add(x.ActorKey, x));
            }

            return _table;
        }
    }


    public Sprite GetPortraitFromKey(string portraitKey)
    {
        foreach (ActorData data in Table.Values)
        {
            if (data.PortraitTable.Table.TryGetValue(portraitKey, out var sprite))
            {
                return sprite;
            }
        }

        return null;
    }
}

[System.Serializable]
public class ActorData
{
    [SerializeField] private string _actorKey;

    [SerializeField] private string _actorName;
    [SerializeField] private FavorabilityEvent _favorabilityEvent;
    [SerializeField] private PortraitTable _portraitTable;

    public string ActorKey => _actorKey;

    public string ActorName => _actorName;
    public FavorabilityEvent FavorabilityEvent => _favorabilityEvent;
    public PortraitTable PortraitTable => _portraitTable;
}
