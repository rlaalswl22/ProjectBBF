using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraUpdater : MonoBehaviour
{
    private CinemachineBrain _brain;
    private CinemachineVirtualCamera _camera;

    private void Awake()
    {
        _brain = GetComponent<CinemachineBrain>();
        _camera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (_brain && (_brain.ActiveVirtualCamera as CinemachineVirtualCamera) == _camera)
        {
            _brain.ManualUpdate();
        }
    }
}
