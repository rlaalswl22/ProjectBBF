using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjectBBF.Persistence
{
    public class BinaryDescriptor : IPersistenceDescriptor
    {
        public void Save(IEnumerable<(string, IPersistenceObject)> toSaveObject)
        {
            foreach ((string, IPersistenceObject) tuple in toSaveObject)
            {
                int size = Marshal.SizeOf(tuple.Item2);
                IntPtr ptr = Marshal.AllocHGlobal(size);
                byte[] arr = new byte[size];

                try
                {
                    Marshal.StructureToPtr(tuple.Item2, ptr, true);
                    Marshal.Copy(ptr, arr, 0, size);

                    arr = AddType(arr, tuple.Item2.GetType());

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(arr);
                    }


                    string encorded = Convert.ToBase64String(arr);

                    PlayerPrefs.SetString(tuple.Item1, encorded);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }

            PlayerPrefs.Save();
        }

        public void LoadPersistenceObject(IEnumerable<(string, IPersistenceObject)> toLoadObject)
        {
            foreach ((string, IPersistenceObject) objTuple in toLoadObject)
            {
                string str = PlayerPrefs.GetString(objTuple.Item1, "");

                if (string.IsNullOrEmpty(str))
                {
                    Debug.LogError($"key({objTuple.Item1}) 데이터 로드 실패");
                    continue;
                }

                byte[] arr = Convert.FromBase64String(str);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(arr);
                }

                (byte[], Type) tuple = SplitType(arr);
                arr = tuple.Item1;
                Type type = tuple.Item2;

                int size = arr.Length;
                IntPtr ptr = Marshal.AllocHGlobal(size);

                try
                {
                    Marshal.Copy(arr, 0, ptr, size);
                    Marshal.PtrToStructure(ptr, objTuple.Item2);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        public List<IPersistenceObject> LoadPersistenceObject(IEnumerable<string> toLoadObject)
        {
            var objList = new List<IPersistenceObject>(10);

            foreach (string key in toLoadObject)
            {
                string str = PlayerPrefs.GetString(key, "");

                if (string.IsNullOrEmpty(str))
                {
                    Debug.LogError($"key({key}) 데이터 로드 실패");
                    continue;
                }

                byte[] arr = Convert.FromBase64String(str);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(arr);
                }

                (byte[], Type) tuple = SplitType(arr);
                arr = tuple.Item1;
                Type type = tuple.Item2;

                int size = arr.Length;
                IntPtr ptr = Marshal.AllocHGlobal(size);

                try
                {
                    Marshal.Copy(arr, 0, ptr, size);
                    var obj = Marshal.PtrToStructure(ptr, type) as IPersistenceObject;

                    if (obj is null)
                    {
                        Debug.LogError($"key({key}) 데이터 로드 실패");
                    }
                    else
                    {
                        objList.Add(obj); 
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }

            return objList;
        }

        private byte[] AddType(byte[] arr, Type type)
        {
            if (type.FullName is null) return null;

            byte[] typeArr = Encoding.Unicode.GetBytes(type.FullName);
            byte[] typeLengthArr = BitConverter.GetBytes(typeArr.Length);
            byte[] newArr = new byte[arr.Length + typeArr.Length + typeLengthArr.Length];

            Array.Copy(typeLengthArr, 0, newArr, 0, typeLengthArr.Length);
            Array.Copy(typeArr, 0, newArr, typeLengthArr.Length, typeArr.Length);
            Array.Copy(arr, 0, newArr, typeLengthArr.Length + typeArr.Length, arr.Length);

            return newArr;
        }

        private (byte[], Type) SplitType(byte[] arr)
        {
            int typeLength = BitConverter.ToInt32(arr, 0);
            int dataLength = arr.Length - typeLength - 4;
            byte[] typeArr = new byte[typeLength];
            byte[] dataArr = new byte[dataLength];
            Array.Copy(arr, 4, typeArr, 0, typeLength);
            Array.Copy(arr, 4 + typeLength, dataArr, 0, dataLength);

            string typeFullName = Encoding.Unicode.GetString(typeArr);

            Type type = Type.GetType(typeFullName);

            return (dataArr, type);
        }
    }
}