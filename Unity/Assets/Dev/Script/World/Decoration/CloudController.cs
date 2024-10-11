using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _distance;

    private Vector3 _backupPos;
    private Vector3 _targetPos;
    private void Start()
    {
        _backupPos = transform.position;
        StartCoroutine(CoUpdate());
    }

    private IEnumerator CoUpdate()
    {
        yield return null;
        
        float t = 0f;
        Vector3 beginPos = _backupPos;
        Vector3 endPos = beginPos + new Vector3(_distance, 0f, 0f);

        while (t <= 1f)
        {
            transform.position = Vector3.Lerp(beginPos, endPos, t);

            t += Time.deltaTime / _duration;

            yield return null;
        }

        StartCoroutine(CoUpdate());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(_distance, 0f, 0f));
    }
}
