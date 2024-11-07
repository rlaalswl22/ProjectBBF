using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // DOTween 네임스페이스 추가

[RequireComponent(typeof(RectTransform))]
public class QuestIndicatorObstacleUI : MonoBehaviour
{
    private Tween _currentTextTween;
    private Tween _currentImageTween;

    private bool isFadingIn = false; // 현재 FadeIn 상태인지 여부
    private bool isFadingOut = false; // 현재 FadeOut 상태인지 여부

    // FadeIn (Alpha 0 -> 1)
    public void FadeIn(float duration)
    {
        // TMP_Text가 있는 경우 FadeIn
        TMP_Text tmpText = GetComponent<TMP_Text>();
        if (tmpText != null && !isFadingIn && !isFadingOut)
        {
            isFadingIn = true;
            tmpText.DOFade(1f, duration).OnKill(() => isFadingIn = false); // 애니메이션 종료 시 상태 복구
        }

        // Image가 있는 경우 FadeIn
        Image image = GetComponent<Image>();
        if (image != null && !isFadingIn && !isFadingOut)
        {
            isFadingIn = true;
            image.DOFade(1f, duration).OnKill(() => isFadingIn = false); // 애니메이션 종료 시 상태 복구
        }
    }

    // FadeOut (Alpha 1 -> 0)
    public void FadeOut(float fadeAlpha, float duration)
    {
        // TMP_Text가 있는 경우 FadeOut
        TMP_Text tmpText = GetComponent<TMP_Text>();
        if (tmpText != null && !isFadingIn && !isFadingOut)
        {
            isFadingOut = true;
            tmpText.DOFade(fadeAlpha, duration).OnKill(() => isFadingOut = false); // 애니메이션 종료 시 상태 복구
        }

        // Image가 있는 경우 FadeOut
        Image image = GetComponent<Image>();
        if (image != null && !isFadingIn && !isFadingOut)
        {
            isFadingOut = true;
            image.DOFade(fadeAlpha, duration).OnKill(() => isFadingOut = false); // 애니메이션 종료 시 상태 복구
        }
    }

    // 현재 FadeIn 상태인지 확인
    public bool IsFadingIn()
    {
        return isFadingIn;
    }

    // 현재 FadeOut 상태인지 확인
    public bool IsFadingOut()
    {
        return isFadingOut;
    }

    private void OnEnable()
    {
        if (QuestManager.Instance)
        {
            QuestManager.Instance.IndicatorObstacleList.Add(this);
        }
    }

    private void OnDisable()
    {
        if (QuestManager.Instance)
        {
            QuestManager.Instance.IndicatorObstacleList.Remove(this);
        }
    }
}
