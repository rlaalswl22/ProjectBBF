using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FrogRaceFrogObject : MonoBehaviour
{
    public bool IsGoal { get; private set; }
    public FrogRaceMinigameData.FrogData FrogData { get; set; }

    public void Begin(FrogRaceMinigameData gameData,  FrogRaceMinigameData.FrogData frogData)
    {
        IsGoal = false;
        StartCoroutine(CoUpdate(gameData, frogData));
    }

    public void End()
    {
        IsGoal = false;
        StopAllCoroutines();
    }

    private IEnumerator CoUpdate(FrogRaceMinigameData gameData,  FrogRaceMinigameData.FrogData frogData)
    {
        var waitJumpInterval = new WaitForSeconds(gameData.JumpInterval);
        
        while (true)
        {
            yield return waitJumpInterval;

            yield return Jump(transform, gameData, frogData);
        }
    }

    private IEnumerator Jump(Transform frogTransform, FrogRaceMinigameData gameData,  FrogRaceMinigameData.FrogData frogData)
    {
        float randomValue = UnityEngine.Random.value;
        if (randomValue > frogData.JumpRate)
        {
            yield break;
        }

        AudioManager.Instance.PlayOneShot("Animal", "Animal_Frog_Jump");
        
        randomValue = UnityEngine.Random.value;
        float t = 0f;
        float boostMultiplier = randomValue <= frogData.JumpBoostRate ? gameData.BoostMovementMultiplier : 1f;
        float maxX = UnityEngine.Random.Range(gameData.JumpMinDistance, gameData.JumpMaxDistance) * boostMultiplier;
        float maxY = gameData.JumpMaxHeight;
        Vector3 backupPos = frogTransform.position;
        
        while (true)
        {
            yield return null;

            float curX = t * maxX;
            float curY = GetCalculatedY(curX, maxX, maxY);
            frogTransform.position = backupPos + new Vector3(curX, curY, 0f); 
            
            t += Time.deltaTime * gameData.MovementSpeed;
            
            if (t >= 1f)
            {
                break;
            }
        }
    }

    private float GetCalculatedY(float x, float maxX, float maxY)
    {
        float b = 4 * maxY / maxX;
        float a = -b / maxX;

        float y = a * (x * x) + b * x;

        return y;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out FrogRaceGoalLine line))
        {
            IsGoal = true;
        }
    }
}