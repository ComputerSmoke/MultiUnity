using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiunityServer
{
    internal class IdGenerator
    {
        int nextId;
        Queue<int> ids;
        int maxId;
        public IdGenerator(int maxId)
        {
            this.maxId = maxId;
            nextId = 1;
        }
        public IdGenerator()
        {
            //Default: 2 byte max int
            this.maxId = 0xFFFF;
        }
        public int Next()
        {
            if (ids.Count > 0) return ids.Dequeue();
            if (nextId >= maxId) throw new Exception("Exceeded maxId of " + maxId);
            return nextId++;
        }
        public void Release(int id)
        {
            ids.Enqueue(id);
        }
    }
}
