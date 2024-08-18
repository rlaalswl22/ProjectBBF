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
        private Queue<(string, IPersistenceObject)> _saveQueue;
        private Queue<(string, IPersistenceObject)> _loadQueue;

        public IPersistenceDescriptor Descriptor { get; set; }
        public bool IsEmpty => _saveQueue.Any() == false;

        private PersistenceTable _keyTable;

        private Dictionary<string, IPersistenceObject> _objTable;
        public IReadOnlyDictionary<string, IPersistenceObject> Table => _objTable;

        public override void PostInitialize()
        {
            _objTable = new Dictionary<string, IPersistenceObject>();

            _saveQueue = new Queue<(string, IPersistenceObject)>(10);
            _loadQueue = new Queue<(string, IPersistenceObject)>(10);

            _keyTable = Resources.Load<PersistenceTable>("Data/PersistenceTable");
        }

        public override void PostRelease()
        {
            _saveQueue.Clear();
            _loadQueue.Clear();

            _objTable = null;

            _saveQueue = null;
            _loadQueue = null;

            Descriptor = null;
        }

        public void Save(bool saveAndFlush = true)
        {
            Descriptor.Save(_saveQueue);

            if (saveAndFlush)
            {
                FlushSaveObject();
            }
        }

        public void LoadWithQueue(bool loadAndFlush = true)
        {
            Descriptor.LoadPersistenceObject(_loadQueue);

            if (loadAndFlush)
            {
                FlushLoadObject();
            }
        }

        public void LoadWithTable()
        {
            var list = Descriptor.LoadPersistenceObject(_keyTable.Keys);

            _objTable.Clear();

            for (int i = 0; i < _keyTable.Keys.Count; i++)
            {
                _objTable[_keyTable.Keys[i]] = list[i];
            }
        }

        public void EnqueueSaveObject(string key, IPersistenceObject obj)
        {
            _saveQueue.Enqueue((key, obj));
        }

        public void EnqueueLoadObject(string key, IPersistenceObject obj)
        {
            _loadQueue.Enqueue((key, obj));
        }

        public void FlushSaveObject()
        {
            _saveQueue.Clear();
        }

        public void FlushLoadObject()
        {
            _loadQueue.Clear();
        }
    }
}