using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TimeHud : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private ESOGameTimeEvent _event;

    private void Awake()
    {
        StartCoroutine(CoUpdate());
    }

    private IEnumerator CoUpdate()
    {
        while (true)
        {
            yield return _event.WaitAsync().WithCancellation(GlobalCancelation.PlayMode);

            var time = TimeManager.Instance.GetGameTime();
            _text.text = $"{time.Hour:D2}:{time.Min:D2} {time.TimeOfDay}";
        }
    }
}
