using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Singleton;
using UnityEngine;

[Singleton(ESingletonType.Global)]
public class InputManager : IGeneralSingleton
{
    private DefaultKeymap _keyMap;

    public void Initialize()
    {
        _keyMap = new DefaultKeymap();
        _keyMap.Enable();

        Actions = _keyMap.PlayerControl;
    }

    public void Release()
    {
        _keyMap.Dispose();
    }


    public static DefaultKeymap.PlayerControlActions Actions { get; private set; }
}
