using System;
using MyBox;
using UnityEngine;

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using System.IO;
#endif

public static class AnimationActorKey
{
    /*
     * Actor 공통
     */
    public static readonly int Up_Idle          = Animator.StringToHash("Up_Idle");
    public static readonly int Down_Idle        = Animator.StringToHash("Down_Idle");
    public static readonly int Left_Idle        = Animator.StringToHash("Left_Idle");
    public static readonly int Right_Idle       = Animator.StringToHash("Right_Idle");
    public static readonly int LeftUp_Idle      = Animator.StringToHash("LeftUp_Idle");
    public static readonly int RightUp_Idle     = Animator.StringToHash("RightUp_Idle");

    public static readonly int Up_Move          = Animator.StringToHash("Up_Move");
    public static readonly int Down_Move        = Animator.StringToHash("Down_Move");
    public static readonly int Left_Move        = Animator.StringToHash("Left_Move");
    public static readonly int Right_Move       = Animator.StringToHash("Right_Move");
    public static readonly int LeftUp_Move      = Animator.StringToHash("LeftUp_Move");
    public static readonly int RightUp_Move     = Animator.StringToHash("RightUp_Move");

    public static readonly int Up_Sprint        = Animator.StringToHash("Up_Sprint");
    public static readonly int Down_Sprint      = Animator.StringToHash("Down_Sprint");
    public static readonly int Left_Sprint      = Animator.StringToHash("Left_Sprint");
    public static readonly int Right_Sprint     = Animator.StringToHash("Right_Sprint");
    public static readonly int LeftUp_Sprint    = Animator.StringToHash("LeftUp_Sprint");
    public static readonly int RightUp_Sprint   = Animator.StringToHash("RightUp_Sprint");


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
    public static int GetAniHash(Movement movement, Direction direction)
    {
        switch (movement)
        {
            case Movement.Idle:
                switch (direction)
                {
                    case Direction.Up: return Up_Idle;
                    case Direction.Down: return Down_Idle;
                    case Direction.Left: return Left_Idle;
                    case Direction.Right: return Right_Idle;
                    case Direction.LeftUp: return LeftUp_Idle;
                    case Direction.RightUp: return RightUp_Idle;
                }
                break;

            case Movement.Walk:
                switch (direction)
                {
                    case Direction.Up: return Up_Move;
                    case Direction.Down: return Down_Move;
                    case Direction.Left: return Left_Move;
                    case Direction.Right: return Right_Move;
                    case Direction.LeftUp: return LeftUp_Move;
                    case Direction.RightUp: return RightUp_Move;
                }
                break;

            case Movement.Sprint:
                switch (direction)
                {
                    case Direction.Up: return Up_Sprint;
                    case Direction.Down: return Down_Sprint;
                    case Direction.Left: return Left_Sprint;
                    case Direction.Right: return Right_Sprint;
                    case Direction.LeftUp: return LeftUp_Sprint;
                    case Direction.RightUp: return RightUp_Sprint;
                }
                break;
        }

        throw new ArgumentException($"Invalid combination of movement: {movement} and direction: {direction}");
    }
}