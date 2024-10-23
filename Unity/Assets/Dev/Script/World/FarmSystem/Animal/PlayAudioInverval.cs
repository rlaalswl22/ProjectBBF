

using System;
using System.Collections;
using UnityEngine;

public class PlayAudioInverval : ActorComponent
{
    [SerializeField] private string _groupKey;
    [SerializeField] private string _audioKey;
    [SerializeField] private float _audioPlayInterval = 10f;
    [SerializeField] private AudioSource _source;
    public void Init(Actor actor)
    {
        StartCoroutine(CoUpdate());
    }

    private IEnumerator CoUpdate()
    {
        if (string.IsNullOrEmpty(_groupKey) || string.IsNullOrEmpty(_audioKey))
        {
            yield break;
        }
        
        string groupKey = _groupKey.Trim();
        string audioKey = _audioKey.Trim();
        
        var wait = new WaitForSeconds(_audioPlayInterval);
        
        while (true)
        {
            yield return wait;

            if (_source)
            {
                _source.PlayOneShot(groupKey, audioKey);
            }
        }
    }
}