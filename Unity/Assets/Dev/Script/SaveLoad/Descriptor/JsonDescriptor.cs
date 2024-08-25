using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjectBBF.Persistence
{
    public class JsonDescriptor : IPersistenceDescriptor
    {
        private const bool PRETTY_PRINT = 
        #if UNITY_EDITOR
            true;
        #else
            false;
        #endif
        
        [Serializable]
        private class Wrapper
        {
            public string Type;
            public string Json;
        }
        
        public void Save(IEnumerable<(string, IPersistenceObject)> toSaveObject)
        {
            foreach ((string, IPersistenceObject) objTuple in toSaveObject)
            {
                string json = JsonUtility.ToJson(objTuple.Item2, PRETTY_PRINT);

                var wrapper = new Wrapper
                {
                    Type = objTuple.Item2.GetType().FullName,
                    Json = json,
                };
                
                json = JsonUtility.ToJson(wrapper, PRETTY_PRINT);

                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogError($"key({objTuple.Item1}) 데이터를 저장하는데 실패");
                    continue;
                }
                
                PlayerPrefs.SetString(objTuple.Item1, json);
            }
        }

        public void LoadPersistenceObject(IEnumerable<(string, IPersistenceObject)> toLoadObject)
        {
            Wrapper wrapper = new Wrapper();
            foreach ((string, IPersistenceObject) objTuple in toLoadObject)
            {
                string json = PlayerPrefs.GetString(objTuple.Item1);
                
                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogError($"key({objTuple.Item1}) 데이터를 불러오는데 실패");
                    continue;
                }
                
                JsonUtility.FromJsonOverwrite(json, wrapper);
                JsonUtility.FromJsonOverwrite(wrapper.Json, objTuple.Item2);
            }
        }

        public List<IPersistenceObject> LoadPersistenceObject(IEnumerable<string> toLoadObject)
        {
            Wrapper wrapper = new Wrapper();
            List<IPersistenceObject> list = new List<IPersistenceObject>(10);
            foreach (string key in toLoadObject)
            {
                string json = PlayerPrefs.GetString(key);
                
                if (string.IsNullOrEmpty(json))
                {
                    list.Add(null);
                    continue;
                }
                
                JsonUtility.FromJsonOverwrite(json, wrapper);

                var type = Type.GetType(wrapper.Type);
                if (type is null)
                {
                    Debug.LogError($"key({key}) 데이터를 불러오는데 실패");
                    list.Add(null);
                    continue;
                }

                IPersistenceObject rtv = Activator.CreateInstance(type) as IPersistenceObject;
                
                if(rtv is null)
                {
                    Debug.LogError($"key({key}) 데이터를 불러오는데 실패");
                    list.Add(null);
                    continue;
                }
                
                list.Add(rtv);
            }

            return list;
        }
    }
}