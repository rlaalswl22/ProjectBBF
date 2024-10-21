using System;
using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Persistence;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageInventoryPresenter : MonoBehaviour, IInventoryPresenter<GridInventoryModel>
{
    [SerializeField] private InteractableInventoryView _playerView;
    [SerializeField] private InteractableInventoryView _storageView;
    public GridInventoryModel PlayerModel { get; private set; }
    public GridInventoryModel Model { get; private set; }

    public InteractableInventoryView PlayerView => _playerView;
    public InteractableInventoryView StorageView => _storageView;

    public event Action<StorageInventoryPresenter> OnInit;

    public void Init()
    {
        StartCoroutine(CoInit());
    }

    private IEnumerator CoInit()
    {
        PlayerView.Init();
        StorageView.Init();

        {
            var blackboard = PersistenceManager.Instance.LoadOrCreate<PlayerBlackboard>("Player_Blackboard");

            while (blackboard.Inventory?.Model is null)
            {
                yield return null;
            }

            PlayerModel = blackboard.Inventory.Model;
        }
        
        Model = new GridInventoryModel(new Vector2Int(10, 2));
        Model.OnChanged += StorageView.Refresh;
        PlayerModel.OnChanged += PlayerView.Refresh;

        PlayerView.OnSlotDown += OnPlayerSlotDown;
        StorageView.OnSlotDown += OnStorageSlotDown;

        PlayerView.Refresh(PlayerModel);
        StorageView.Refresh(Model);
        PlayerView.Visible = false;
        StorageView.Visible = false;

        OnInit?.Invoke(this);
    }

    private void OnPlayerSlotDown(IInventorySlot obj, PointerEventData eventData)
    {
        if (InputManager.Map.UI.SlotQuickMove.IsPressed() && obj.Data)
        {
            InventoryHelper.QuickMoveWithGridModel(obj, Model);
        }
        else
        {
            InventoryHelper.SwapOrHalfItem(obj, eventData);
        }
    }
    private void OnStorageSlotDown(IInventorySlot obj, PointerEventData eventData)
    {
        if (InputManager.Map.UI.SlotQuickMove.IsPressed() && obj.Data)
        {
            InventoryHelper.QuickMoveWithGridModel(obj, PlayerModel);
        }
        else
        {
            InventoryHelper.SwapOrHalfItem(obj, eventData);
        }
    }

    private void OnDestroy()
    {
        if (Model is not null)
        {
            Model.OnChanged -= StorageView.Refresh;
            PlayerModel.OnChanged -= PlayerView.Refresh;
        }
    }
}