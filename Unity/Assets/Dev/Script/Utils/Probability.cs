using System.Collections.Generic;
using UnityEngine;

public static class ProbabilityHelper
{
    /// <summary>
    /// 주어진 확률 리스트에 따라 랜덤하게 하나의 인덱스를 반환하는 함수
    /// </summary>
    /// <param name="probabilities">0~1 사이의 확률 값을 가진 리스트 (합이 1이어야 함)</param>
    /// <returns>선택된 확률의 인덱스</returns>
    public static int GetRandomIndex(List<float> probabilities)
    {
        // 확률 리스트가 유효한지 확인
        if (probabilities == null || probabilities.Count == 0)
        {
            return -1;
        }

        // 확률 값들의 합을 계산 (1이어야 함)
        float totalSum = 0f;
        foreach (var probability in probabilities)
        {
            totalSum += probability;
        }

        // 확률 값들의 합이 1이 아니면 에러
        if (Mathf.Abs(totalSum - 1f) > 0.01f)
        {
            Debug.LogError("확률 값들의 합이 1이 아닙니다.");
            return -1;
        }

        // 0과 1 사이의 랜덤 값 생성
        float randomPoint = Random.Range(0f, 1f);

        // 확률에 따라 랜덤한 인덱스를 찾음
        float cumulativeSum = 0f;
        for (int i = 0; i < probabilities.Count; i++)
        {
            cumulativeSum += probabilities[i];
            if (randomPoint <= cumulativeSum)
            {
                return i;
            }
        }

        // 이 경우는 절대 발생하지 않겠지만, 방어적으로 -1 반환
        return -1;
    }
}