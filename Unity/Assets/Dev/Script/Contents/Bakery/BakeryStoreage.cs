using System;
using System.Collections;
using ProjectBBF.Event;
using UnityEngine;

public class BakeryStoreage : BakeryFlowBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private StorageInventoryPresenter _storageInventory;
    [SerializeField] private StorageInventoryPresenter _playernventory;

    private int _initCount;
    private void Start()
    {
        _storageInventory.OnInit += _ => Init();
        _playernventory.OnInit += _ => Init();
        
        _storageInventory.Init();
        _playernventory.Init();
    }

    private void Init()
    {
        _initCount++;

        if (_initCount >= 2)
        {
            Visible = false;
        }
    }

    private bool Visible
    {
        get => _panel.activeSelf;
        set
        {
            _panel.SetActive(value);
            _storageInventory.View.Visible = value;
            _playernventory.View.Visible = value;
        }
    }

    protected override void OnActivate(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
    }

    protected override void OnInteraction(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
        if (activator.Owner is not PlayerController pc) return;

        var inputAction = InputManager.Map.Player.Interaction;
        if (inputAction.triggered)
        {
            Visible = !Visible;
            pc.Blackboard.IsInteractionStopped = Visible;
            pc.Blackboard.IsMoveStopped = Visible;
        }
    }

    protected override void OnEnter(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
    }

    protected override void OnExit(BakeryFlowObject flowObject, CollisionInteractionMono activator)
    {
        if (activator.Owner is not PlayerController pc) return;

        Visible = false;
        pc.Blackboard.IsInteractionStopped = false;
        pc.Blackboard.IsMoveStopped = false;
    }
}