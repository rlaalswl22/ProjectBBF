using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BlackAlphaScreenDirector : ScreenDirector
{
    [SerializeField] private string _key;
    [SerializeField] private Image _panel;
    [SerializeField] private float _fadeoutDuration;
    [SerializeField] private float _fadeinDuration;
    [SerializeField] private float _waitDuration;

    public override string Key => _key;

    private float _savedVolume;

    public override bool Enabled
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }

    public override UniTask Fadein()
    {
        _panel.color = Color.black;

        var c = Color.black;
        c.a = 0f;

        var sequence = DOTween.Sequence();

        sequence.Join(
            _panel
                .DOColor(c, _fadeinDuration).SetEase(Ease.Linear)
                .SetDelay(_waitDuration)
                .SetId(this)
        );
        sequence.Join(
            DOVirtual.Float(
                    0f,
                    _savedVolume,
                    _fadeinDuration,
                    x => AudioManager.Instance.SetVolume("BGM", x))
                .SetEase(Ease.OutQuad)
        );

        return sequence
                .Play()
                .AsyncWaitForCompletion()
                .AsUniTask()
                .WithCancellation(GlobalCancelation.PlayMode)
            ;
    }

    public override UniTask Fadeout()
    {
        var c = Color.black;
        c.a = 0f;
        _panel.color = c;

        c.a = 1f;

        var sequence = DOTween.Sequence();
        _savedVolume = AudioManager.Instance.GetVolume("BGM");

        sequence.Join(
            _panel
                .DOColor(c, _fadeoutDuration).SetEase(Ease.Linear)
                .SetId(this)
        );
        sequence.Join(
            DOVirtual.Float(
                    _savedVolume, 
                    0f,
                    _fadeinDuration, 
                    x => AudioManager.Instance.SetVolume("BGM", x))
                .SetEase(Ease.InQuad)
        );

        return sequence
                .Play()
                .AsyncWaitForCompletion()
                .AsUniTask()
                .WithCancellation(GlobalCancelation.PlayMode)
            ;
    }
}