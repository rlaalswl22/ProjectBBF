using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjectBBF.Persistence
{
    public abstract class DataTagAttribute : Attribute {}

    public class GameDataAttribute : DataTagAttribute {}
    public class UserDataAttribute : DataTagAttribute {}
    
    public class DoNotEditableAttribute : DataTagAttribute {}
    public class EditableAttribute : DataTagAttribute {}
    public class PersistenceListAttribute : DataTagAttribute {}

    public interface ISaveLoadNotification
    {
        public void OnSavedNotify();
        public void OnLoadedNotify();
    }
}