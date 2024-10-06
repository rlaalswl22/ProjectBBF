using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeHudUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _dayText;
    [SerializeField] private ESOGameTimeEvent _event;

    public bool Visible
    {
        get=> gameObject.activeSelf;
        set=> gameObject.SetActive(value);
    }
    private void Awake()
    {
        StartCoroutine(CoUpdate());
    }

    private IEnumerator CoUpdate()
    {
        GameTime before = new GameTime(-1, -1);
        
        while (true)
        {
            var cur = TimeManager.Instance.GetGameTime();
            int curDay = TimeManager.Instance.SaveData.Day;
            if (cur != before)
            {
                _timeText.text = $"Day {curDay:D3}";
                _dayText.text = $"{cur.Hour:D2}:{cur.Min:D2} {cur.TimeOfDay}";
                before = cur;
            }

            yield return null;
        }
    }
}
