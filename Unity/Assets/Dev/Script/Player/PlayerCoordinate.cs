using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerCoordinate : MonoBehaviour, IPlayerStrategy
{
    private PlayerController _controller; 
    
    public void Init(PlayerController controller)
    {
        _controller = controller;
    }

    public void OnUpdate()
    {
    }

    public Vector3 GetFront()
    {
        var worldPos = _controller.transform.position;

        worldPos += (Vector3)_controller.MoveStrategy.LastMovedDirection;

        return worldPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetFront(), 2f);
    }
}
