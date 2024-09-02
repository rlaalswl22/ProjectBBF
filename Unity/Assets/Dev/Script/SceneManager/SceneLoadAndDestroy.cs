using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadAndDestroy : MonoBehaviour
{
    private static bool _isLoaded;

    [RuntimeInitializeOnLoadMethod]
    private static void OnInit()
    {
        _isLoaded = false;
    }
    private void Awake()
    {
        if (_isLoaded)
        {
            Destroy(gameObject);
        }

        _isLoaded = true;
    }
}
