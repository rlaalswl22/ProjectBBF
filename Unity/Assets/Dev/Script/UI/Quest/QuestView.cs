


using TMPro;
using UnityEngine;

public class QuestView : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    
    public QuestData Data { get; private set; }
    
    public void SetData(QuestData data)
    {
        Debug.Assert(data);

        Data = data;
        _title.text = data.Title;
        _description.text = data.Description;
    }

    public void Clear()
    {
        Data = null;
        _title.text = "";
        _description.text = "";
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}