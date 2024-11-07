using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class QuestIndicatorUI : ActorComponent
{
    [SerializeField] private Color _fadeUiColor;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private ESOQuest _esoQuest;
    [SerializeField] private GameObject _renderer;
    [SerializeField] private Transform _pivot;

    [SerializeField] private List<QuestData> _targetQuestList;

    private Camera _cachedCamera;
    private Actor _actor;
    private Canvas _canvas;
    private bool _isWorldCanvas;

    private Camera CachedCamera
    {
        get
        {
            if (_cachedCamera == false)
            {
                _cachedCamera = Camera.main;
            }

            return _cachedCamera;
        }
    }
    
    public void Init(Actor actor)
    {
        if (_esoQuest == false) return;

        _actor = actor;

        _esoQuest.OnEventRaised += OnEventRaised;
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        
        RectTransform boundTransform = _renderer.transform as RectTransform;
        _savedScale = boundTransform.localScale;
    }

    private void OnDestroy()
    {
        if (_esoQuest == false) return;
        
        _esoQuest.OnEventRaised -= OnEventRaised;
        print(name);
    }

    private void OnEventRaised(QuestEvent obj)
    {
        if (this == false)
        {
            _esoQuest.OnEventRaised -= OnEventRaised;
            return;
        }
        
        if (_targetQuestList.FirstOrDefault(x => x.QuestKey == obj.QuestKey) == false) return;
        
        if (obj.Type is QuestType.Create)
        {
            gameObject.SetActive(true);
        }
        else if(obj.Type is QuestType.Cancele or QuestType.Complete)
        {
            gameObject.SetActive(false);
        }
        
    }

    private Vector2 _savedScale;
    private void UpdatePosition(bool isWorldCanvas)
    {
        RectTransform boundTransform = _renderer.transform as RectTransform;
        Camera camera = CachedCamera;
        
        Debug.Assert(boundTransform);
        Debug.Assert(camera);
        Debug.Assert(_renderer);
        Debug.Assert(_pivot);

        float scaleFactor = isWorldCanvas ? (0.5f * camera.scaledPixelHeight / camera.orthographicSize) : 1f;
        scaleFactor *= _canvas.scaleFactor;

        Vector2 cameraScaledSize = new Vector2(camera.scaledPixelWidth, camera.scaledPixelHeight);
        Vector2 boundSize = boundTransform.rect.size * boundTransform.lossyScale * scaleFactor;
        Vector2 pos = camera.WorldToScreenPoint(_pivot.position);
        pos = ScreenUtils.ScreenToOrthogonal(pos, cameraScaledSize);

        pos = ScreenUtils.ToValidPosition(
            boundSize,
            cameraScaledSize,
            pos
        );
        
        if (QuestManager.Instance)
        {
            foreach (QuestIndicatorObstacleUI obstacle in QuestManager.Instance.IndicatorObstacleList)
            {
                if (Input.GetKey(KeyCode.C))
                {
                    if (obstacle.IsFadingOut() is false)
                    {
                        obstacle.FadeOut(0.2f, 0.2f);
                        DOTween.Kill(this);
                        boundTransform.DOScale(_savedScale * 1.5f, 0.3f).SetId(this).SetLoops(-1, LoopType.Yoyo);
                    }
                }
                else
                {
                    if (obstacle.IsFadingIn() is false)
                    {
                        DOTween.Kill(this);
                        obstacle.FadeIn(0.2f);
                        boundTransform.DOScale(_savedScale, 0.3f).SetId(this);
                    }
                }
            }
        }
        
        pos = ScreenUtils.OrthogonalToScreen(pos, cameraScaledSize);

        if (isWorldCanvas)
        {
            pos = camera.ScreenToWorldPoint(pos);
        }



        boundTransform.position = Vector2.Lerp(boundTransform.position, pos, Time.deltaTime / _duration);
    }

    private void LateUpdate()
    {
        UpdatePosition(_canvas.renderMode == RenderMode.WorldSpace);
    }
}
