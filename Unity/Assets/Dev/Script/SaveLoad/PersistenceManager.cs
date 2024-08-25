using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectBBF.Singleton;
using UnityEngine;

namespace ProjectBBF.Persistence
{
    [Singleton(ESingletonType.Global)]
    public class PersistenceManager : MonoBehaviourSingleton<PersistenceManager>
    {
        public IPersistenceDescriptor Descriptor { get; set; } = new JsonDescriptor();

        private Dictionary<string, IPersistenceObject> _objTable;

        public override void PostInitialize()
        {
            _objTable = new Dictionary<string, IPersistenceObject>();
        }

        public override void PostRelease()
        {
            _objTable.Clear();
            _objTable = null;

            Descriptor = null;
        }

        private string[] _tempKeyArr = new string[1];
        
        public void Save()
        {
            Descriptor.Save(_objTable.Select(x => (x.Key, x.Value)));
        }
        
        public T LoadOrCreate<T>(string key) where T : class, IPersistenceObject, new()
        {
            IPersistenceObject cachedObj = GetCachedPersistenceObj(ref key);
            if (cachedObj is null)
            {
                _tempKeyArr[0] = key;       
                var list = Descriptor.LoadPersistenceObject(_tempKeyArr);
                
                cachedObj = list.FirstOrDefault();

                if (cachedObj is null)
                {
                    cachedObj = new T();
                    Debug.Log($"PersistenceManager object 생성(key: {typeof(T)}, type: {key})");
                }
                
                _objTable[key] = cachedObj;
            }

            return (T)cachedObj;
        }

        public IPersistenceObject GetCachedPersistenceObj(ref string key)
        {
            if (_objTable.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
        }
    }
}