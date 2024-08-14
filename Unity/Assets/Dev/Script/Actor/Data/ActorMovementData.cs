



using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Actor/ActorMovementData", fileName = "NewActorMovementData")]
public class ActorMovementData : ScriptableObject
{
    [SerializeField] private float _movementSpeed;

    public float MovementSpeed => _movementSpeed;
}