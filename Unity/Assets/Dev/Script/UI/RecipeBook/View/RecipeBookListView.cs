using System;
using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookListView : MonoBehaviour
{
    [SerializeField] private bool _awakeAndDisable = true;
    
    [SerializeField] private GameObject _slotViewPrefab;
    [SerializeField] private Transform _content;

    [SerializeField] private ESOVoid _closeEvent;
    [SerializeField] private ESOVoid _closeReadyEvent;
    
    public event Action<object> OnSlotClick;
    public event Action OnExit;

    private List<RecipeBookSlotView> _slots = new();

    public List<RecipeBookSlotView> Slots => _slots;

    private bool _canCloseEventRaise;

    public bool Visible
    {
        get => gameObject.activeSelf;
        set
        {
            gameObject.SetActive(value);

            if (_canCloseEventRaise && value is false && _closeEvent)
            {
                _canCloseEventRaise = false;
                _closeEvent.Raise();
            }
        }
    }

    private void OnEventReady()
    {
        _canCloseEventRaise = true;
    }
    
    private void Awake()
    {
        Visible = !_awakeAndDisable;

        if(_closeReadyEvent)
            _closeReadyEvent.OnEventRaised += OnEventReady;

        for (int i = 0; i < _content.childCount; i++)
        {
            Destroy(_content.GetChild(i).gameObject);
        }
    }

    private void OnDestroy()
    {
        foreach (var slot in _slots)
        {
            slot.OnClick -= OnSlotClick;
        }
        _slots.Clear();
        

        if(_closeReadyEvent)
            _closeReadyEvent.OnEventRaised -= OnEventReady;
    }

    private RecipeBookSlotView CreateSlotView()
    {
        var obj = Instantiate(_slotViewPrefab, _content);
        var slotView = obj.GetComponent<RecipeBookSlotView>();
        
        Debug.Assert(slotView);

        return slotView;
    }

    public void AddItem(Sprite itemSprite, object data, bool isUnlocked)
    {
        var slot = CreateSlotView();
        slot.SetData(itemSprite, data, isUnlocked);
        slot.OnClick += OnSlotClicked;
        _slots.Add(slot);
    }

    private void OnSlotClicked(RecipeBookSlotView slotView)
    {
        OnSlotClick?.Invoke(slotView.Data);
    }
}
