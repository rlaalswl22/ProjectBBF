using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Actor/ActorAnimationData", fileName = "NewActorAnimationData")]
public class AnimationData : ScriptableObject
{
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

    public AnimationClip IdleUp => _idleUp;

    public AnimationClip IdleDown => _idleDown;

    public AnimationClip IdleLeft => _idleLeft;

    public AnimationClip IdleRight => _idleRight;

    public AnimationClip IdleLeftUp => _idleLeftUp;

    public AnimationClip IdleRightUp => _idleRightUp;

    public AnimationClip MovementUp => _movementUp;

    public AnimationClip MovementDown => _movementDown;

    public AnimationClip MovementLeft => _movementLeft;

    public AnimationClip MovementRight => _movementRight;

    public AnimationClip MovementLeftUp => _movementLeftUp;

    public AnimationClip MovementRightUp => _movementRightUp;
}
