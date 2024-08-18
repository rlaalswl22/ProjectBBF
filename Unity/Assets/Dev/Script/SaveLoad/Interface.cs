using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjectBBF.Persistence
{
    public interface IBasePersistenceOwner<TObject> 
        where TObject : IPersistenceObject
    {
        public void FromPresistenceObject(TObject persistenceObject);
        public TObject ToPersistenceObject();
    }

    public interface IPersistenceObject
    {
    }


    public interface IPersistenceDescriptor
    {
        public void Save(IEnumerable<(string, IPersistenceObject)> toSaveObject);

        [CanBeNull]
        public void LoadPersistenceObject(IEnumerable<(string, IPersistenceObject)> toLoadObject);
        public List<IPersistenceObject> LoadPersistenceObject(IEnumerable<string> toLoadObject);
    }
}