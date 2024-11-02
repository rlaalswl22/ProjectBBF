using System;
using UnityEngine;
using UnityEngine.UI;



public static class ScreenUtils
{
    [Flags]
    public enum Direction : int
    {
        None = 0,
        Up = 1,
        Down = 2,
        Right = 4,
        Left = 8
    }

    public static void SetPositionWithOffset(Transform transform, Vector2 screenPosition, Vector2 offset)
    {
        transform.position = screenPosition + offset;
    }

    public static Direction GetOverlappedDirectionScreen(Vector2 orthogonalPos, Vector2 size, Vector2 cameraScaledPixelSize)
    {
        cameraScaledPixelSize -= size * 2f;
        Bounds myBounds = new Bounds(orthogonalPos, size);
        Bounds screenBounds = new Bounds(Vector3.zero, cameraScaledPixelSize);


        Direction dir = Direction.None;

        if (screenBounds.Intersects(myBounds))
        {
            return dir;
        }
        
        Bounds tempA = new Bounds(Vector3.zero, new Vector3(cameraScaledPixelSize.x, 0f, 0f));
        Bounds tempB = new Bounds(new Vector3(orthogonalPos.x, 0f, 0f), new Vector3(size.x, 0f, 0f));
        if (tempA.Intersects(tempB) is false)
        {
            if (orthogonalPos.x > 0f)
            {
                dir |= Direction.Right;
            }
            else
            {
                dir |= Direction.Left;
            }
        }
        
        tempA = new Bounds(Vector3.zero, new Vector3(0f, cameraScaledPixelSize.y, 0f));
        tempB = new Bounds(new Vector3(0f, orthogonalPos.y, 0f), new Vector3(0f, size.y, 0f));
        if (tempA.Intersects(tempB) is false)
        {
            if (orthogonalPos.y > 0f)
            {
                dir |= Direction.Up;
            }
            else
            {
                dir |= Direction.Down;
            }
        }


        return dir;
    }

    public static Vector2 OrthogonalToScreen(Vector2 screenPoint, Vector2 cameraScaledPixelSize)
    {
        return new Vector3
        (
            screenPoint.x + cameraScaledPixelSize.x * 0.5f,
            screenPoint.y + cameraScaledPixelSize.y * 0.5f
        );
    }
    public static Vector2 ScreenToOrthogonal(Vector2 screenPoint, Vector2 cameraScaledPixelSize)
    {
        return new Vector3
        (
            screenPoint.x - cameraScaledPixelSize.x * 0.5f,
            screenPoint.y - cameraScaledPixelSize.y * 0.5f
        );
    }

    public static Vector2 ToValidPosition(Vector2 boundSize, Vector2 cameraScaledPixelSize, Vector3 orthogonalPos)
    {
        var dir = GetOverlappedDirectionScreen(orthogonalPos, boundSize, cameraScaledPixelSize);
        var cameraSize = new Vector2(cameraScaledPixelSize.x, cameraScaledPixelSize.y);

        
        if ((dir & Direction.Left) == Direction.Left)
        {
            orthogonalPos.x = -cameraSize.x * 0.5f + boundSize.x * 0.5f;
        }
        if ((dir & Direction.Right) == Direction.Right)
        {
            orthogonalPos.x = cameraSize.x * 0.5f - boundSize.x * 0.5f;
        }
        if ((dir & Direction.Up) == Direction.Up)
        {
            orthogonalPos.y = cameraSize.y * 0.5f - boundSize.y * 0.5f;
        }
        if ((dir & Direction.Down) == Direction.Down)
        {
            orthogonalPos.y = -cameraSize.y * 0.5f + boundSize.y * 0.5f;
        }

        return orthogonalPos;
    }
}