using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ProjectBBF/Data/FadeinoutObject", fileName = "New FadeinoutObject")]
public class FadeinoutObjectData : ScriptableObject
{
    [SerializeField] private float _innerRadius;
    [SerializeField] private float _outterRadius;
    [SerializeField] private Ease _ease;

    public float InnerRadius => _innerRadius;

    public float OutterRadius => _outterRadius;

    public Ease Ease => _ease;
}