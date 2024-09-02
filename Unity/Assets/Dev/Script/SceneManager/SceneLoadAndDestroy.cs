using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadAndDestroy : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject);
    }
}
