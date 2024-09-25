using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "ProjectBBF/Audio/AudioTable", fileName = "New audio table")]
public class AudioTable : ScriptableObject
{
    [SerializeField] private string _tableKey;
    [SerializeField] private AudioMixerGroup _group;
    [SerializeField] private List<Set> _list;

    public string TableKey => _tableKey;
    public AudioMixerGroup MixerGroup => _group;
    public IReadOnlyList<Set> List => _list;


    [ButtonMethod]
    private void CheckKeyCollision()
    {
        Dictionary<string, HashSet<int>> container = new();

        for (int i = 0; i < List.Count; i++)
        {
            for (int j = 0; j < List.Count; j++)
            {
                if (i == j) continue;

                if (List[i].Key == List[j].Key)
                {
                    if (container.TryGetValue(List[i].Key, out var hashSet) is false)
                    {
                        hashSet = new HashSet<int>();
                        container.Add(List[i].Key, hashSet);
                    }
                    
                    hashSet.Add(j);
                    hashSet.Add(i);
                }
            }
        }

        if (container.Any() is false)
        {
            Debug.Log("Key collision check");
            return;
        }

        string str = "SoundTable key is collision.\n";

        foreach (var hashSet in container)
        {
            str += $"==== Key({hashSet.Key}) collision list ====\n";
            foreach (int index in hashSet.Value)
            {
                string audioClipName = List[index].Audio ? List[index].Audio.name : "null";
                str += $"{{audio name({audioClipName}), key({List[index].Key})}}\n";
            }
        }
        
        Debug.LogError(str);
    }


    [Serializable]
    public struct Set
    {
        public AudioClip Audio;
        public string Key;
    }
}