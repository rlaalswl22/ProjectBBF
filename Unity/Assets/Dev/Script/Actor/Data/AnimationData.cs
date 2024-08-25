using System;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Data/Actor/ActorAnimationData", fileName = "NewActorAnimationData")]
public class AnimationData : ScriptableObject
{
    [Serializable]
    public enum Movement
    {
        Idle,
        Walk,
        Sprint
    }

    [Serializable]
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        LeftUp,
        RightUp
    }
    
    [field: SerializeField, Foldout("대기"), OverrideLabel("위")]
    protected AnimationClip _idleUp;
    [field: SerializeField, Foldout("대기"), OverrideLabel("아래")]
    protected AnimationClip _idleDown;
    [field: SerializeField, Foldout("대기"), OverrideLabel("왼쪽")]
    protected AnimationClip _idleLeft;
    [field: SerializeField, Foldout("대기"), OverrideLabel("오른쪽")]
    protected AnimationClip _idleRight;
    [field: SerializeField, Foldout("대기"), OverrideLabel("왼쪽 위")]
    protected AnimationClip _idleLeftUp;
    [field: SerializeField, Foldout("대기"), OverrideLabel("오른쪽 위")]
    protected AnimationClip _idleRightUp;
    
    [field: SerializeField, Foldout("이동"), OverrideLabel("위")]
    protected AnimationClip _movementUp;
    [field: SerializeField, Foldout("이동"), OverrideLabel("아래")]
    protected AnimationClip _movementDown;
    [field: SerializeField, Foldout("이동"), OverrideLabel("왼쪽")]
    protected AnimationClip _movementLeft;
    [field: SerializeField, Foldout("이동"), OverrideLabel("오른쪽")]
    protected AnimationClip _movementRight;
    [field: SerializeField, Foldout("이동"), OverrideLabel("왼쪽 위")]
    protected AnimationClip _movementLeftUp;
    [field: SerializeField, Foldout("이동"), OverrideLabel("오른쪽 위")]
    protected AnimationClip _movementRightUp;

    [field: SerializeField, Foldout("달리기"), OverrideLabel("위")]
    protected AnimationClip _sprintUp;
    [field: SerializeField, Foldout("달리기"), OverrideLabel("아래")]
    protected AnimationClip _sprintDown;
    [field: SerializeField, Foldout("달리기"), OverrideLabel("왼쪽")]
    protected AnimationClip _sprintLeft;
    [field: SerializeField, Foldout("달리기"), OverrideLabel("오른쪽")]
    protected AnimationClip _sprintRight;
    [field: SerializeField, Foldout("달리기"), OverrideLabel("왼쪽 위")]
    protected AnimationClip _sprintLeftUp;
    [field: SerializeField, Foldout("달리기"), OverrideLabel("오른쪽 위")]
    protected AnimationClip _sprintRightUp;

    public AnimationClip GetClip(Movement movement, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                switch (movement)
                {
                    case Movement.Idle:
                        return _idleUp;
                    case Movement.Walk:
                        return _movementUp;
                    case Movement.Sprint:
                        return _sprintUp;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                }
            case Direction.Down:
                switch (movement)
                {
                    case Movement.Idle:
                        return _idleDown;
                    case Movement.Walk:
                        return _movementDown;
                    case Movement.Sprint:
                        return _sprintDown;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                }
            case Direction.Left:
                switch (movement)
                {
                    case Movement.Idle:
                        return _idleLeft;
                    case Movement.Walk:
                        return _movementLeft;
                    case Movement.Sprint:
                        return _sprintLeft;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                }
            case Direction.Right:
                switch (movement)
                {
                    case Movement.Idle:
                        return _idleRight;
                    case Movement.Walk:
                        return _movementRight;
                    case Movement.Sprint:
                        return _sprintRight;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                }
            case Direction.LeftUp:
                switch (movement)
                {
                    case Movement.Idle:
                        return _idleLeftUp;
                    case Movement.Walk:
                        return _movementLeftUp;
                    case Movement.Sprint:
                        return _sprintLeftUp;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                }
            case Direction.RightUp:
                switch (movement)
                {
                    case Movement.Idle:
                        return _idleRightUp;
                    case Movement.Walk:
                        return _movementRightUp;
                    case Movement.Sprint:
                        return _sprintRightUp;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                }
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        throw new ArgumentException();
    }
}
