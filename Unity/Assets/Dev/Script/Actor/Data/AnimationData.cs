using System;
using MyBox;
using UnityEngine;

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using System.IO;
#endif

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

#if UNITY_EDITOR
    private void SetClip(Movement movement, Direction direction, AnimationClip clip)
    {
        switch (direction)
        {
            case Direction.Up:
                switch (movement)
                {
                    case Movement.Idle:
                        _idleUp = clip;
                        break;
                    case Movement.Walk:
                        _movementUp = clip;
                        break;
                    case Movement.Sprint:
                        _sprintUp = clip;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                }

                break;
            case Direction.Down:
                switch (movement)
                {
                    case Movement.Idle:
                        _idleDown = clip;
                        break;
                    case Movement.Walk:
                        _movementDown = clip;
                        break;
                    case Movement.Sprint:
                        _sprintDown = clip;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                }

                break;
            case Direction.Left:
                switch (movement)
                {
                    case Movement.Idle:
                        _idleLeft = clip;
                        break;
                    case Movement.Walk:
                        _movementLeft = clip;
                        break;
                    case Movement.Sprint:
                        _sprintLeft = clip;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                }

                break;
            case Direction.Right:
                switch (movement)
                {
                    case Movement.Idle:
                        _idleRight = clip;
                        break;
                    case Movement.Walk:
                        _movementRight = clip;
                        break;
                    case Movement.Sprint:
                        _sprintRight = clip;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                }

                break;
            case Direction.LeftUp:
                switch (movement)
                {
                    case Movement.Idle:
                        _idleLeftUp = clip;
                        break;
                    case Movement.Walk:
                        _movementLeftUp = clip;
                        break;
                    case Movement.Sprint:
                        _sprintLeftUp = clip;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                }

                break;
            case Direction.RightUp:
                switch (movement)
                {
                    case Movement.Idle:
                        _idleRightUp = clip;
                        break;
                    case Movement.Walk:
                        _movementRightUp = clip;
                        break;
                    case Movement.Sprint:
                        _sprintRightUp = clip;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    [ButtonMethod]
    private void Load()
    {
        string folderPath =
            EditorUtility.OpenFolderPanel("Select a file", "Assets/Dev/Art/UAnimation", "Assets/Dev/Art/UAnimation");
        var clips = GetClips(folderPath);

        foreach (var clip in clips)
        {
            if (TryGetDirection(clip.name, out Direction direction) is false) continue;
            if (TryGetMovement(clip.name, out Movement movement) is false) continue;
            
            SetClip(movement, direction, clip);
        }
    }

    private static List<AnimationClip> GetClips(string folderPath)
    {
        List<AnimationClip> clips = new();

        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogError("유효하지 않은 경로");
            return clips;
        }

        string[] animationPaths = Directory.GetFiles(folderPath, "*.anim", SearchOption.AllDirectories);

        // 애니메이션 파일을 필터링
        foreach (string path in animationPaths)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);

            if (IsValidAnimationFile(fileName))
            {
                string assetPath = GetAssetPathFromFile(path);
                AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);

                if (clip != null)
                {
                    Debug.Log("Loaded Animation Clip: " + assetPath);
                    clips.Add(clip);
                }
                else
                {
                    Debug.LogError("Failed to load Animation Clip: " + assetPath);
                }
            }
        }

        return clips;
    }

    private static bool IsValidAnimationFile(string fileName)
    {
        // 예: @Clip_Up_Idle.anim
        // 파일 이름이 특정 패턴을 따르는지 검사
        return fileName.StartsWith("@Clip_") &&
               (fileName.Contains("_Up_") || fileName.Contains("_Down_") ||
                fileName.Contains("_Right_") || fileName.Contains("_Left_") ||
                fileName.Contains("_RightUp_") || fileName.Contains("_LeftUp_")) &&
               (fileName.Contains("_Idle") || fileName.Contains("_Move") || fileName.Contains("_Sprint"));
    }
    private static bool TryGetDirection(string fileName, out Direction direction)
    {
        direction = default;

        if (fileName.Contains("_Up_"))
        {
            direction = Direction.Up;
        }
        else if (fileName.Contains("_Down_"))
        {
            direction = Direction.Down;
        }
        else if (fileName.Contains("_Left_"))
        {
            direction = Direction.Left;
        }
        else if (fileName.Contains("_Right_"))
        {
            direction = Direction.Right;
        }
        else if (fileName.Contains("_LeftUp_"))
        {
            direction = Direction.LeftUp;
        }
        else if (fileName.Contains("_RightUp_"))
        {
            direction = Direction.RightUp;
        }
        else
        {
            return false; // 방향을 찾지 못했음
        }

        return true;
    }

    private static bool TryGetMovement(string fileName, out Movement movement)
    {
        movement = default;

        if (fileName.Contains("_Idle"))
        {
            movement = Movement.Idle;
        }
        else if (fileName.Contains("_Move"))
        {
            movement = Movement.Walk;
        }
        else if (fileName.Contains("_Sprint"))
        {
            movement = Movement.Sprint;
        }
        else
        {
            return false; // 동작 상태를 찾지 못했음
        }

        return true;
    }

    private static string GetAssetPathFromFile(string filePath)
    {
        // 파일 경로를 AssetDatabase의 경로로 변환
        string relativePath = "Assets" + filePath.Substring(Application.dataPath.Length);
        return relativePath;
    }
#endif
}