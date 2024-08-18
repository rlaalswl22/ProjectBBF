using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MyBox;
using UnityEngine;

namespace ProjectBBF.Persistence.Test
{
    using ProjectBBF.Persistence;
    
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class PersistenceTestObject : IPersistenceObject
    {
        public int Data1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Data2;
        public float Data3;
        public bool Data4;
    }
    
    public class PersistenceTest : MonoBehaviour
    {
        [SerializeField] private List<string> _keyList;
        [SerializeField] private List<PersistenceTestObject> _objList;

        private readonly IPersistenceDescriptor Descriptor = new BinaryDescriptor();

        private void Awake()
        {
            Save();
            Clear();
            _objList.Add(new PersistenceTestObject());
            LoadWithQueue();
            Clear();
        }

        [ButtonMethod]
        private void Save()
        {
            if (Application.isPlaying == false) return;
            
            var instance = PersistenceManager.Instance;
    
            for (int i = 0; i < Mathf.Min(_keyList.Count, _objList.Count); i++)
            {
                instance.EnqueueSaveObject(_keyList[i], _objList[i]);
            }
    
            instance.Descriptor = Descriptor;
            instance.Save(false);  
            instance.FlushSaveObject();
        }
        [ButtonMethod]
        private void Clear()
        {
            if (Application.isPlaying == false) return;
            _objList.Clear();
        }
        
        [ButtonMethod]
        private void LoadWithQueue()
        {
            if (Application.isPlaying == false) return;
            
            var instance = PersistenceManager.Instance;
            
            for (int i = 0; i < Mathf.Min(_keyList.Count, _objList.Count); i++)
            {
                instance.EnqueueLoadObject(_keyList[i], _objList[i]);
            }
            
            instance.Descriptor = Descriptor;
            instance.LoadWithQueue(false);  
            instance.FlushLoadObject();
        }
    }

}