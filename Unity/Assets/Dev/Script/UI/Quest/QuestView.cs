


using System;
using System.Collections;
using DG.Tweening;
using MyBox;
using TMPro;
using UnityEngine;

public class QuestView : MonoBehaviour
{
    [SerializeField] private float _hideDuration;
    [SerializeField] private float _appearDuration;

    [SerializeField] private float _hideAmplitude = 1f;
    [SerializeField] private float _appearAmplitude = 1f;

    [SerializeField] private float _hidePeriod = 1f;
    [SerializeField] private float _appearPeriod = 1f;

    [SerializeField] private Ease _hideEase;
    [SerializeField] private Ease _appearEase;
    
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private RectTransform _target;
    
    public QuestData Data { get; private set; }
    private Tweener _tweener;

    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(CoAnimate(true, false));
    }

    private IEnumerator CoAnimate(bool fadein, bool fadeoutAndDestroy = false)
    {
        float hiddenX = transform.position.x - _target.rect.size.x;
        float appearedX = transform.position.x;

        float begin = fadein ? hiddenX : appearedX;
        float end = fadein ? appearedX : hiddenX;
        float duration = fadein ? _appearDuration : _hideDuration;
        float amplitude = fadein ? _appearAmplitude : _hideAmplitude;
        float period = fadein ? _appearPeriod : _hidePeriod;
        Ease ease = fadein ? _appearEase : _hideEase;
        
        _target.SetX(begin);
        _tweener?.Kill(true);
        _tweener = _target.DOMoveX(end, duration).SetEase(ease, amplitude, period);
        yield return _tweener.WaitForCompletion();

        if (fadeoutAndDestroy && fadein is false)
        {
            _tweener?.Kill(true);
            Destroy(gameObject);
        }
    }

    public void SetData(QuestData data)
    {
        Debug.Assert(data);

        Data = data;
        _title.text = data.Title;
        _description.text = data.Description;
    }

    public void Clear()
    {
        Data = null;
        _title.text = "";
        _description.text = "";
    }

    public void DestroySelf()
    {
        StopAllCoroutines();
        StartCoroutine(CoAnimate(false, true));
    }
}