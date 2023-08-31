using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiunity.Shared
{
    public class IdGenerator<T>
    {
        int nextId;
        Queue<int> ids;
        int maxId;
        Dictionary<T, int> idMapping;
        Dictionary<int, T> idMapRev;
        public IdGenerator(int maxId)
        {
            this.maxId = maxId;
            nextId = 1;
            idMapping = new Dictionary<T,int>();
            idMapRev = new Dictionary<int,T>();
            ids = new Queue<int>();
        }
        public IdGenerator()
        {
            //Default: 2 byte max int
            this.maxId = 0xFFFF;
            nextId = 1;
            idMapping = new Dictionary<T, int>();
            idMapRev = new Dictionary<int, T>();
            ids = new Queue<int>();
        }
        public int Assign(T obj)
        {
            if (!idMapping.ContainsKey(obj))
            {
                idMapping[obj] = Next();
                idMapRev[idMapping[obj]] = obj;
            }
            return idMapping[obj];
        }
        private int Next()
        {
            if (ids.Count > 0) return ids.Dequeue();
            if (nextId >= maxId) throw new Exception("Exceeded maxId of " + maxId);
            return nextId++;
        }
        public void Release(int id)
        {
            if (!idMapRev.ContainsKey(id)) return;
            idMapping.Remove(idMapRev[id]);
            idMapRev.Remove(id);
            ids.Enqueue(id);
        }
        public int GetId(T obj)
        {
            return idMapping[obj];
        }
    }
}
