using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalAnimator : ActorComponent
{
    [Serializable]
    private struct ValueSlider
    {
        public string Key;
        public float Value;
        public RangedFloat WairDuration;
    }

    [SerializeField] private RangedFloat _startDelay;
    [SerializeField, InitializationField] private List<ValueSlider> _sliders;
    
    private Animator _animator;
    
    
    
    public void Init(Actor actor)
    {
        _animator = actor.Animator;

        CheckValid();

        StartCoroutine(CoUpdate());
    }

    private void CheckValid()
    {
        float total = 0f;
        foreach (var slider in _sliders)
        {
            total += slider.Value;
        }

        if (total > 1f)
        {
            Debug.LogWarning("Slider의 총 합 값이 1을 초과합니다.");
        }
    }


    private IEnumerator CoUpdate()
    {
        yield return new WaitForSeconds(Random.Range(_startDelay.Min, _startDelay.Max));
        
        List<(WaitForSeconds waits, string key, float probability)> waits =_sliders.Select(x=> (new WaitForSeconds(Random.Range(x.WairDuration.Min, x.WairDuration.Max)), x.Key, x.Value)).ToList();
        List<float> probabilities = _sliders.Select(x => x.Value).ToList();

        while (true)
        {
            int index = ProbabilityHelper.GetRandomIndex(probabilities);
            if (index == -1)
            {
                break;
            }

            var wait = waits[index];

            if (_animator)
            {
                _animator.SetTrigger(wait.key);
            }
                
            yield return wait.waits;
        }
    }
    
    
}
