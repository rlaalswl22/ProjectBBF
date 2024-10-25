using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ScrollbarElementTracker : MonoBehaviour
{
    public Scrollbar scrollbar;         // Scrollbar를 에디터에서 연결
    public RectTransform viewport;   // 스크롤되는 콘텐츠의 RectTransform
    public RectTransform contentRect;   // 스크롤되는 콘텐츠의 RectTransform

    private RectTransform selfRect;     // 자신의 RectTransform
    private float contentHeight;
    private float viewportHeight;

    private float _before;

    private void Start()
    {
        Invoke("Init", 0.1f);

    }

    private void Init()
    {
        // 자기 자신의 RectTransform을 selfRect로 설정
        selfRect = GetComponent<RectTransform>();

        // 콘텐츠와 뷰포트의 높이를 계산
        contentHeight = contentRect.rect.height;
        viewportHeight = viewport.rect.height;
        _before = scrollbar.value;

        // Scrollbar 값이 변경될 때마다 UpdatePosition 함수 호출
        scrollbar.onValueChanged.AddListener(UpdatePosition);
        UpdatePosition(scrollbar.value); // 초기 위치 설정
    }

    public void UpdatePosition(float scrollValue)
    {
        if (contentHeight <= viewportHeight) return;

        // 스크롤 값을 통해 콘텐츠 내 위치 계산
        float newY = selfRect.anchoredPosition.y + (_before - scrollValue) * (contentHeight - viewportHeight);
        _before = scrollValue;
        
        // 새로운 위치를 적용하여 자신을 이동
        selfRect.anchoredPosition = new Vector2(selfRect.anchoredPosition.x, newY);
    }
}