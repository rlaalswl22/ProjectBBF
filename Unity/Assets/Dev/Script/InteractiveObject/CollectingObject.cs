using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using MyBox;
using UnityEngine;

[RequireTag("CollectingObject")]
[RequireComponent(typeof(CollisionInteraction))]
public class CollectingObject : MonoBehaviour
{
    #region Properties

    [field: SerializeField, InitializationField, MustBeAssigned, AutoProperty]
    private CollisionInteraction _interaction;

    [field: SerializeField, Separator("커스텀"), OverrideLabel("데이터"), InitializationField, MustBeAssigned,
            DisplayInspector]
    private CollectingObjectData _data;

    [field: SerializeField, InitializationField, DisplayInspector]
    private List<CollectingObjectBehaviour> _behaviours;

    #endregion

    #region Getter/Setter

    public CollisionInteraction Interaction => _interaction;
    public CollectingObjectData Data => _data;
    public IReadOnlyList<CollectingObjectBehaviour> Behaviour => _behaviours;

    #endregion

    private void Awake()
    {
        var info = ObjectContractInfo.Create(() => gameObject);
        _interaction.SetContractInfo(info, this);

        foreach (CollectingObjectBehaviour behaviour in _behaviours)
        {
            if (behaviour == false) continue;
            
            behaviour.InitBehaviour(_data, _interaction, info);
        }
    }
}