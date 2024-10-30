using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectBBF.Persistence
{
    [UserData, Serializable]
    public class GameSetting
    {
        [SerializeField] private float _volumeBGM;
        [SerializeField] private float _volumeSFX;
        [SerializeField] private float _volumePlayer;
        [SerializeField] private float _volumeAnimal;
        [SerializeField] private float _volumeUI;

        [SerializeField] private int _vsyncCount;
        [SerializeField] private int _screenMode;
        [SerializeField] private Vector2Int _resolution;
        [SerializeField] private int _refreshRate;

        public float VolumeBGM
        {
            get => _volumeBGM;
            set => _volumeBGM = value;
        }

        public float VolumeSFX
        {
            get => _volumeSFX;
            set => _volumeSFX = value;
        }

        public float VolumePlayer
        {
            get => _volumePlayer;
            set => _volumePlayer = value;
        }

        public float VolumeAnimal
        {
            get => _volumeAnimal;
            set => _volumeAnimal = value;
        }

        public float VolumeUI
        {
            get => _volumeUI;
            set => _volumeUI = value;
        }

        public int VsyncCount
        {
            get => _vsyncCount;
            set => _vsyncCount = value;
        }

        public int ScreenMode
        {
            get => _screenMode;
            set => _screenMode = value;
        }

        public Vector2Int Resolution
        {
            get => _resolution;
            set => _resolution = value;
        }
        public int RefreshRate
        {
            get => _refreshRate;
            set => _refreshRate = value;
        }
    }
}