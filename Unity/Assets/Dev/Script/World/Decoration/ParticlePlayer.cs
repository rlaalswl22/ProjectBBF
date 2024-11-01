


using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    private List<ParticleSystem> _particles;

    public void Realloc()
    {
        if (_particles is null)
        {
            _particles = new List<ParticleSystem>(2);
        }
        
        _particles.Clear();
        _particles.AddRange(GetComponentsInChildren<ParticleSystem>(true));

        foreach (ParticleSystem particle in _particles)
        {
            particle.Stop();
        }
    }
    
    public void Play()
    {
        if (_particles is null)
        {
            Realloc();
        }

        foreach (ParticleSystem particle in _particles)
        {
            particle.Play();
        }
    }

    public void Stop()
    {
        if (_particles is null) return;

        foreach (ParticleSystem particle in _particles)
        {
            particle.Play();
        }
    }
}