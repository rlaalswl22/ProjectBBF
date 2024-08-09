using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Singleton;
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

[Singleton(ESingletonType.Global, -10)]
public class ActorDataManager : MonoBehaviourSingleton<ActorDataManager>
{
    public ActorDataTable Table { get; private set; }

    public override void PostInitialize()
    {
        Table = Resources.Load<ActorDataTable>("Data/ActorDataTable");
        Debug.Assert(Table is not null);
    }

    public override void PostRelease()
    {
        Table = null;
    }
}
