



using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using MyBox;
using ProjectBBF.Event;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(CollisionInteraction))]
public class FadeinoutObject : MonoBehaviour
{
    [field: SerializeField, AutoProperty, InitializationField, MustBeAssigned]
    private CollisionInteraction _interaction;
    
    [SerializeField] private FadeinoutObjectData _data;
    
    public event Action<float> OnFadeAlpha;
    public event Action<CollisionInteractionMono> OnEnter;
    public event Action<CollisionInteractionMono> OnStay;
    public event Action<CollisionInteractionMono> OnExit;

    public CollisionInteraction Interaction => _interaction;

    private void Awake()
    {
        var rigid = GetComponent<Rigidbody2D>();
        rigid.isKinematic = true;

        var col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = _data.OutterRadius;

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") is false) return;
        if (other.TryGetComponent(out CollisionInteractionMono interaction) is false) return;
        
        StopAllCoroutines();
        StartCoroutine(CoFade(interaction.transform));
        
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
        
        StopAllCoroutines();
        StartCoroutine(CoFade(interaction.transform));
        
        OnExit?.Invoke(interaction);
    }

    private IEnumerator CoFade(Transform targetTransform)
    {
        
        
        while (true)
        {
            float dis = Vector2.Distance(targetTransform.position, transform.position);
            dis = Mathf.Clamp(dis - _data.InnerRadius, 0f, _data.OutterRadius - _data.InnerRadius);
            dis /= (_data.OutterRadius - _data.InnerRadius);
            
            
            float evaluedValue = EaseManager.ToEaseFunction(_data.Ease).Invoke(dis, 1f, 0f, 0f);
            OnFadeAlpha?.Invoke(1f - evaluedValue);
                
            yield return null;
        }
    }

}