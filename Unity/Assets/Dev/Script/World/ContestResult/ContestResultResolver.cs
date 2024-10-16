

using System.Collections.Generic;
using System.Linq;
using ProjectBBF.Singleton;
using UnityEngine;

[Singleton(ESingletonType.Global)]
public class ContestResultResolver : MonoBehaviourSingleton<ContestResultResolver>
{
    private const string DATA_PATH = "Data/ContestResultTable";

    private ContestResultData _data;
    
    public override void PostInitialize()
    {
        _data = Resources.Load<ContestResultData>(DATA_PATH);
        
        if (_data == false)
        {
            Debug.LogError("데이터 테이블을 찾을 수 없음");
            return;
        }
    }

    public override void PostRelease()
    {
        _data = null;
    }

    public bool TryResolve(string itemKey, ref List<ContestResultData.Record> results)
    {
        if (results is null) return false;

        results.Clear();
        foreach (var record in _data.Table)
        {
            if (record.Item && record.Item.ItemKey == itemKey)
            {
                results.Add(record);
            }
        }

        return results.Any();
    }
    public bool TryResolve(ItemData itemData, ref List<ContestResultData.Record> results)
    {
        return TryResolve(itemData.ItemKey, ref results);
    }
}