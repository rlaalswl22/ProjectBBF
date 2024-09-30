using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookListView : MonoBehaviour
{
    [SerializeField] private bool _awakeAndDisable = true;
    
    [SerializeField] private GameObject _slotViewPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private Button _exitBtn;
    
    public event Action<object> OnSlotClick;
    public event Action OnExit;

    private List<RecipeBookSlotView> _slots = new();

    public List<RecipeBookSlotView> Slots => _slots;

    public bool Visible
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }
    
    private void Awake()
    {
        Visible = !_awakeAndDisable;

        _exitBtn.onClick.AddListener(() =>
        {
            OnExit?.Invoke();
            Visible = false;
        });

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
