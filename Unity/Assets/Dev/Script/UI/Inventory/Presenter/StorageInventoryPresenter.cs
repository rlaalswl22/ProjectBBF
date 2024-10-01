using System;
using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using UnityEngine;

public class StorageInventoryPresenter : MonoBehaviour, IInventoryPresenter<GridInventoryModel>
{
    [SerializeField] private bool _isPlayerOwner;
    [SerializeField] private InteractableInventoryView _view;
    public GridInventoryModel Model { get; private set; }

    public InteractableInventoryView View => _view;

    public event Action<StorageInventoryPresenter> OnInit;

    public void Init()
    {
        StartCoroutine(CoInit());
    }

    private IEnumerator CoInit()
    {
        _view.Init();
        
        if (_isPlayerOwner)
        {
            var blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");

            while (blackboard.Inventory?.Model is null)
            {
                yield return null;
            }

            Model = blackboard.Inventory.Model;
        }
        else
        {
            Model = new GridInventoryModel(new Vector2Int(10, 2));
        }

        Model.OnChanged += View.Refresh;

        View.Refresh(Model);
        View.Visible = false;
        
        OnInit?.Invoke(this);
    }

    private void OnDestroy()
    {
        if (Model is not null)
        {
            Model.OnChanged -= View.Refresh;
        }
    }
}