using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using DG.Tweening;
using DS.Runtime;
using TMPro;
using UnityEngine.UI;

public class DialogueView : MonoBehaviour
{
    [SerializeField] private float _textCompletionDuration = 0.35f;
    
    [SerializeField] private RectTransform _skipArrow;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private TMP_Text _displayNameText;
    [SerializeField] private Image _portrait;

    [SerializeField] private float _skipTwinkleDuration = 0.5f;
    [SerializeField] private float _skipTwinkleX = 0.35f;
    [SerializeField] private Ease _skipTwinkleEase = Ease.Unset;

    [SerializeField] private Transform _fieldContent;
    [SerializeField] private List<DialogueBranchField> _branchFields;

    private Dictionary<Type, DialogueBranchField> _fieldTable;
    public float TextCompletionDuration => _textCompletionDuration;
    
    public IReadOnlyDictionary<Type, DialogueBranchField> BranchFields => _fieldTable;

    public Transform FieldContent => _fieldContent;

    public string DialogueText
    {
        get => _dialogueText.text;
        set => _dialogueText.text = value;
    }
    public string DisplayName
    {
        get => _displayNameText.text;
        set => _displayNameText.text = value;
    }

    public bool Visible
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    public void SetTextVisible(bool value)
    {
        _dialogueText.gameObject.SetActive(value);
    }

    private void Awake()
    {
        for (int i = 0; i < _fieldContent.childCount; i++)
        {
            var child = _fieldContent.GetChild(i);
            if (child)
            {
                Destroy(child.gameObject);
            }
        }
        
        SetTextVisible(true);
        

        _fieldTable = new();
        Debug.Assert(_branchFields is not null);

        foreach (var field in _branchFields)
        {
            _fieldTable.Add(field.GetType(), field);
        }

    }

    public void SetPortrait(Sprite sprite)
    {
        _portrait.sprite = sprite;

        _portrait.gameObject.SetActive(sprite is not null);
    }
    
    private IEnumerator CoAnimateSkipArrow()
    {
        float x = _skipArrow.anchoredPosition.x;
        while (true)
        {
            yield return _skipArrow
                    .DOAnchorPosX(x - _skipTwinkleX, _skipTwinkleDuration)
                    .SetEase(_skipTwinkleEase)
                    .WaitForCompletion()
                ;
            yield return _skipArrow
                    .DOAnchorPosX(x, _skipTwinkleDuration)
                    .SetEase(_skipTwinkleEase)
                    .WaitForCompletion()
                ;
        }
    } 
}
