



using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CircleCollider2D))]
public class FadeinoutObject : MonoBehaviour
{
    [field: SerializeField, InitializationField, MustBeAssigned]
    private CollisionInteraction _interaction;
    [field: SerializeField, InitializationField, MustBeAssigned]
    private Rigidbody2D _rigid;
    
    [SerializeField] private FadeinoutObjectData _data;
    
    public event Action<float> OnFadeAlpha;
    public event Action<CollisionInteractionMono> OnEnter;
    public event Action<CollisionInteractionMono> OnStay;
    public event Action<CollisionInteractionMono> OnExit;

    public CollisionInteraction Interaction => _interaction;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        
        if(_rigid)
            _rigid.isKinematic = true;

        var col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = _data.OutterRadius;

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void OnChangedCloserObject(CollisionInteractionMono changed)
    {
        StopAllCoroutines();
        StartCoroutine(CoFade(changed == Interaction));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") is false) return;
        if (other.TryGetComponent(out CollisionInteractionMono interaction) is false) return;
        
        if (Interaction && other.TryGetComponent(out PlayerController pc))
        {
            pc.Interactor.AddCloserObject(Interaction);
            pc.Interactor.OnChangedCloserObject += OnChangedCloserObject;

            if (pc.Interactor.CloserObject == Interaction)
            {
                StopAllCoroutines();
                StartCoroutine(CoFade(true));
            }
        }
        
        
        OnEnter?.Invoke(interaction);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") is false) return;
        if (other.TryGetComponent(out CollisionInteractionMono interaction) is false) return;
        
        OnStay?.Invoke(interaction);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") is false) return;
        if (other.TryGetComponent(out CollisionInteractionMono interaction) is false) return;
        
        if (Interaction && other.TryGetComponent(out PlayerController pc))
        {
            pc.Interactor.OnChangedCloserObject -= OnChangedCloserObject;
            pc.Interactor.RemoveCloserObject(Interaction);
        }
        
        StopAllCoroutines();

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(CoFade(false));
        }
        
        OnExit?.Invoke(interaction);
    }

    private float _lastT = 0f;
    private IEnumerator CoFade(bool fadein, bool continuous = true)
    {
        float dir = fadein ? 1f : -1f;
        float t = fadein ? 0f : 1f;

        if (continuous)
        {
            t = _lastT;
        }
        
        while (true)
        {
            float evaluatedValue = EaseManager.ToEaseFunction(_data.Ease).Invoke(t, 1f, 0f, 0f);
            
            OnFadeAlpha?.Invoke(evaluatedValue);

            if (t is > 1f or < 0f)
            {
                t = Mathf.Clamp01(t);
                _lastT = t;
                break;
            }
            
            t += dir * Time.deltaTime / _data.FadeDuration;
            _lastT = t;
            
            yield return null;
        }
    }

}