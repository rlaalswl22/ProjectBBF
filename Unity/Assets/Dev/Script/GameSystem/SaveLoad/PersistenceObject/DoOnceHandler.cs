

using System.Collections.Generic;

namespace ProjectBBF.Persistence
{
    [System.Serializable, GameData]
    public class DoOnceHandlerPersistenceObject
    {
        public List<string> DoOnceList = new List<string>();
    }
}