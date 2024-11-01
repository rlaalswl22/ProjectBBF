


using System.Collections.Generic;
using System.Linq;
using ProjectBBF.Persistence;
using ProjectBBF.Singleton;
using UnityEngine;

[Singleton(ESingletonType.Global)]
public class QuestManager : MonoBehaviourSingleton<QuestManager>
{
    private const string TABLE_PATH = "Data/Quest";

    public Dictionary<string, QuestData> Table { get; private set; }

    public const string PERSISTENCE_KEY = "Quest";
    public const string ESO_PATH = "Event/ESO_Quest";
    
    public ESOQuest ESO { get; private set; }

    public override void PostInitialize()
    {
        ESO = Resources.Load<ESOQuest>(ESO_PATH);
        QuestData[] arr = Resources.LoadAll<QuestData>(TABLE_PATH);

        Table = new Dictionary<string, QuestData>(arr.Select(x => new KeyValuePair<string, QuestData>(x.QuestKey, x)));
        
        Debug.Assert(ESO);
    }

    public override void PostRelease()
    {
        Table?.Clear();
        Table = null;
    }
}