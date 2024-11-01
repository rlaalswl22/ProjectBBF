using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Event/MinigameBegin", fileName = "New Minigame begin")]
public class ESOMinigame : ESOVoid
{
    [SerializeField] private string _minigameKey;
    public string MinigameKey => _minigameKey;
}