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

        private readonly IPersistenceDescriptor Descriptor = new JsonDescriptor();
    }

}