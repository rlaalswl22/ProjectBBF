using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/QuestData", fileName = "New Quest Data")]
public class QuestData : ScriptableObject
{
    [SerializeField] private string _questKey;
    [SerializeField] private string _title;
    [SerializeField] private string _description;

    public string QuestKey => _questKey;
    public string Title => _title;
    public string Description => _description;
}