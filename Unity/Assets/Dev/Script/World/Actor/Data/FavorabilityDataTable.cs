using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Singleton;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Singleton(ESingletonType.Global, -10)]
public class ActorDataManager : MonoBehaviourSingleton<ActorDataManager>
{
    private Dictionary<string, FavorabilityData> _cachedTable;

    public Dictionary<string, FavorabilityData> CachedDict=> _cachedTable;

    public override void PostInitialize()
    {
        var resList = Addressables.LoadResourceLocationsAsync("ActorFavor", typeof(FavorabilityData)).WaitForCompletion();
        _cachedTable = new();

        if (resList is null)
        {
            Debug.LogError("Resources Location을 찾을 수 없음.");
            return;
        }

        var favorList = Addressables.LoadAssetsAsync<FavorabilityData>(resList, null).WaitForCompletion();

        if (favorList is null)
        {
            Debug.LogError("favoraData를 찾을 수 없음.");
            return;
        }
        
        foreach (var favorabilityData in favorList)
        {
            if (_cachedTable.TryGetValue(favorabilityData.ActorKey, out var alreadyData))
            {
                Debug.LogError($"key({favorabilityData.ActorKey}) file({favorabilityData.name})가 이미 존재. 기존 key({alreadyData.ActorKey}) file({alreadyData.name})");
                continue;
            }
            
            _cachedTable.Add(favorabilityData.ActorKey, favorabilityData);
        }
    }

    public override void PostRelease()
    {
        _cachedTable = null;
    }
    
    public Sprite GetPortraitFromKey(string portraitKey)
    {
        if (string.IsNullOrEmpty(portraitKey)) return null;
        
        foreach (FavorabilityData data in _cachedTable.Values)
        {
            if (data.PortraitTable.Table.TryGetValue(portraitKey, out var sprite))
            {
                return sprite;
            }
        }

        return null;
    }
}
