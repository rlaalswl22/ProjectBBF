using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemToolTipView : MonoBehaviour
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

    [SerializeField] private RectTransform _boundTransform;
    [SerializeField] private TMP_Text _itemNameDisplayText;
    [SerializeField] private TMP_Text _itemDescriptionDisplayText;
    
    public string ItemNameDisplayText
    {
        get => _itemNameDisplayText.text;
        set => _itemNameDisplayText.text = value;
    }
    public string ItemDescriptionDisplayText
    {
        get => _itemDescriptionDisplayText.text;
        set => _itemDescriptionDisplayText.text = value;
    }

    public void SetText(ItemData item)
    {
        if (item is null)
        {
            Clear();
            Debug.LogError("ItemToolTipView setText item null");
            return;
        }
        
        ItemNameDisplayText = item.ItemName;
        ItemDescriptionDisplayText = item.ItemDescription;
    }

    public void Clear()
    {
        ItemNameDisplayText = string.Empty;
        ItemDescriptionDisplayText = string.Empty;
    }

    public bool Visible
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    public static Direction GetOverlappedDirectionScreen(Vector2 worldPos, Vector2 size)
    {
        Camera cam = Camera.main;

        var cameraSize = new Vector2(cam.scaledPixelWidth, cam.scaledPixelHeight);

        
        Bounds myBounds = new Bounds(new Vector3(worldPos.x - cameraSize.x * 0.5f, worldPos.y - cameraSize.y * 0.5f), size);
        Bounds leftScreenBounds = new Bounds(Vector3.left * cameraSize.x, cameraSize);
        Bounds rightScreenBounds = new Bounds(Vector3.right * cameraSize.x, cameraSize);
        Bounds upScreenBounds = new Bounds(Vector3.up * cameraSize.y, cameraSize);
        Bounds downScreenBounds = new Bounds(Vector3.down * cameraSize.y, cameraSize);

        Direction dir = Direction.None;

        if (leftScreenBounds.Intersects(myBounds))
        {
            dir |= Direction.Left;
        }
        if (rightScreenBounds.Intersects(myBounds))
        {
            dir |=  Direction.Right;
        }
        if (upScreenBounds.Intersects(myBounds))
        {
            dir |=  Direction.Up;
        }
        if (downScreenBounds.Intersects(myBounds))
        {
            dir |=  Direction.Down;
        }

        return dir;
    }

    private static Vector2 WorldToScreen(Vector2 screenPoint)
    {
        Camera cam = Camera.main;
        var cameraSize = new Vector2(cam.scaledPixelWidth, cam.scaledPixelHeight);
        return new Vector3
        (
            screenPoint.x - cameraSize.x * 0.5f,
            screenPoint.y - cameraSize.y * 0.5f
        );
    }
    private static Vector2 ScreenToWorld(Vector2 screenPoint)
    {
        Camera cam = Camera.main;
        var cameraSize = new Vector2(cam.scaledPixelWidth, cam.scaledPixelHeight);
        return new Vector3
        (
            screenPoint.x + cameraSize.x * 0.5f,
            screenPoint.y + cameraSize.y * 0.5f
        );
    }

    public Vector2 ToValidPosition(Vector3 pos)
    {
        Camera cam = Camera.main;
        var dir = GetOverlappedDirectionScreen(pos, _boundTransform.rect.size * 2f);
        var size = _boundTransform.rect.size * 2f;
        var cameraSize = new Vector2(cam.scaledPixelWidth, cam.scaledPixelHeight);

        pos = WorldToScreen(pos);
        
        if ((dir & Direction.Left) == Direction.Left)
        {
            pos.x = -cameraSize.x * 0.5f + size.x * 0.5f;
        }
        if ((dir & Direction.Right) == Direction.Right)
        {
            pos.x = cameraSize.x * 0.5f - size.x * 0.5f;
        }
        if ((dir & Direction.Up) == Direction.Up)
        {
            pos.y = cameraSize.y * 0.5f - size.y * 0.5f;
        }
        if ((dir & Direction.Down) == Direction.Down)
        {
            pos.y = -cameraSize.y * 0.5f + size.y * 0.5f;
        }

        return ScreenToWorld(pos);
    }

    private void Awake()
    {
        Visible = false;
    }
}
