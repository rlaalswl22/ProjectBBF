using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField] private Transform _cloudTransform;
    [SerializeField] private float _duration;
    [SerializeField] private float _distance;
    [SerializeField, HideInInspector] public float _t;

    private Vector3 _backupPos;
    private Vector3 _targetPos;

    public float SliderValue
    {
        get => _t * Mathf.Max(_distance, 0.0001f);
        set => _t = value / Mathf.Max(_distance, 0.0001f);
    }
    public float Distance => _distance;

    private void Start()
    {
        _backupPos = Vector3.zero;

        if (_cloudTransform)
        {
            StartCoroutine(CoUpdate());
        }
    }

    private IEnumerator CoUpdate()
    {
        _t = 0f;
        Vector3 beginPos = _backupPos;
        Vector3 endPos = beginPos + new Vector3(_distance, 0f, 0f);

        while (_t <= 1f)
        {
            _cloudTransform.localPosition = Vector3.Lerp(beginPos, endPos, _t);

            _t += Time.deltaTime / _duration;

            yield return null;
        }

        StartCoroutine(CoUpdate());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(_distance, 0f, 0f));
    }

    public void OnEditorUpdate()
    {
        if (_cloudTransform == false) return;
        
        Vector3 beginPos = _backupPos;
        Vector3 endPos = beginPos + new Vector3(_distance, 0f, 0f);

        _cloudTransform.localPosition = Vector3.Lerp(beginPos, endPos, _t);
    }
}
