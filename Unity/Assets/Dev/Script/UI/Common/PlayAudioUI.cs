using UnityEngine;

public class PlayAudioUI : MonoBehaviour
{
    public void Play(string audioKey)
    {
        AudioManager.Instance.PlayOneShot("UI", audioKey);   
    }

    public void Play(string groupKey, string audioKey)
    {
        AudioManager.Instance.PlayOneShot(groupKey, audioKey);
    }
}