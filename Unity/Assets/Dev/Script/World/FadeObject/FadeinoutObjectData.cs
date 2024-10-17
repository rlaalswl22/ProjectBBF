using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ProjectBBF/Data/FadeinoutObject", fileName = "New FadeinoutObject")]
public class FadeinoutObjectData : ScriptableObject
{
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _outterRadius;
    [SerializeField] private Ease _ease;

    public float FadeDuration => _fadeDuration;
    public float OutterRadius => _outterRadius;

    public Ease Ease => _ease;
}