

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

    public void Resolve(int chapter, string itemKey, ref List<ContestResultData.Record> results)
    {
        if (results is null) return;

        results.Clear();
        foreach (var record in _data.Table)
        {
            if(record.Chapter != chapter)continue;
            if (record.Item && record.Item.ItemKey == itemKey)
            {
                results.Add(record);
            }
        }

        if (results.Any() is false)
        {
            GetFailure(ref results);
        }
    }
    public void Resolve(int chapter, ItemData itemData, ref List<ContestResultData.Record> results)
    {
        Resolve(chapter, itemData?.ItemKey, ref results);
    }

    public void GetFailure(ref List<ContestResultData.Record> results)
    {
        results.Clear();
        _data.ExceptionRecords.ForEach(results.Add);
    }
}