



using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioClipBinder : MonoBehaviour
{
    [SerializeField] private string _bgmKey;

    private void Awake()
    {
       var source =  GetComponent<AudioSource>();
       source.clip = AudioManager.Instance.GetAudio("BGM", _bgmKey).clip;

       if (source.playOnAwake)
       {
           source.Play();
       }
    }
}