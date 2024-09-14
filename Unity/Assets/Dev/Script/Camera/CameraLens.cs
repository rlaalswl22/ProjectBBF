using Cinemachine;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLens : MonoBehaviour
{
    [field: SerializeField, MustBeAssigned, InitializationField, AutoProperty]
    private CinemachineVirtualCamera _camera;

    [SerializeField] private float _ppu;
    [SerializeField] private float _size;

    private void Awake()
    {
        LensUpdate();
    }

    private void LateUpdate()
    {
        LensUpdate();
    }

    public void LensUpdate()
    {
        var lens = _camera.m_Lens;
        lens.OrthographicSize = (Screen.height / _ppu) * 0.5f * _size;
        _camera.m_Lens = lens;
    }
}
