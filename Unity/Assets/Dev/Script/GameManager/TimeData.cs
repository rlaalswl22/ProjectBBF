using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ProjectBBF/Project/TimeData", fileName = "TimeData")]
public class TimeData : ScriptableObject
{
    [SerializeField] private float _scale;
    [SerializeField] private GameTime _morningTime;

    [SerializeField] private GameTime _definitionPM;
    [SerializeField] private GameTime _definitionNight;
    [SerializeField] private GameTime _definitionEndOfDay;

    public float Scale => _scale;

    public GameTime MorningTime => _morningTime;

    public GameTime DefinitionPm => _definitionPM;

    public GameTime DefinitionNight => _definitionNight;

    public GameTime DefinitionEndOfDay => _definitionEndOfDay;
}