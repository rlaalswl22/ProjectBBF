using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Singleton;
using UnityEngine;
using UnityEngine.Audio;

[Singleton(ESingletonType.Global)]
public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    private const string PATH = "Audio/";

    private Dictionary<string, (AudioMixerGroup mixerGroup, AudioSource source, Dictionary<string, AudioClip> dict)> _audioTables;
    

    public override void PostInitialize()
    {
        var list = Resources.LoadAll<AudioTable>(PATH);

        _audioTables = new();

        foreach (var table in list)
        {
            Dictionary<string, AudioClip> audioClip = new();

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = table.MixerGroup;
            _audioTables.Add(table.TableKey, (table.MixerGroup, audioSource, audioClip));
            foreach (var set in table.List)
            {
                audioClip.Add(set.Key, set.Audio);
            }
        }

    }

    public override void PostRelease()
    {
        _audioTables = null;
    }

    public (AudioSource source, AudioClip clip) GetAudio(string tableKey, string audioKey, bool logError = true)
    {
        var table = _audioTables.GetValueOrDefault(tableKey);

        var clip = table.dict?.GetValueOrDefault(audioKey);

        if (logError && clip == false)
        {
            Debug.LogWarning($"table key({tableKey}), audioKey ({audioKey})");
        }
        
        return (table.source, clip);
    }

    public bool Play(string tableKey, string audioKey)
    {
        var tuple = GetAudio(tableKey, audioKey);
        if (tuple.clip == false) return false;

        tuple.source.clip = tuple.clip;
        tuple.source.Play();
        return true;
    }

    public bool PlayScheduled(string tableKey, string audioKey, double time)
    {
        var tuple = GetAudio(tableKey, audioKey);
        if (tuple.clip == false) return false;
        
        tuple.source.clip = tuple.clip;
        tuple.source.PlayScheduled(time);
        return true;
    }

    public bool PlayDelayed(string tableKey, string audioKey, float delay)
    {
        var tuple = GetAudio(tableKey, audioKey);
        if (tuple.clip == false) return false;
        
        tuple.source.clip = tuple.clip;
        tuple.source.PlayDelayed(delay);
        return true;
    }

    public bool PlayOneShot(string tableKey, string audioKey, float? volumeScale = null)
    {
        var tuple = GetAudio(tableKey, audioKey);
        if (tuple.clip == false) return false;
        
        tuple.source.clip = tuple.clip;

        if (volumeScale.HasValue)
        {
            tuple.source.PlayOneShot(tuple.clip, volumeScale.Value);
        }
        else
        {
            tuple.source.PlayOneShot(tuple.clip);
        }
        return true;
    }
    
}
