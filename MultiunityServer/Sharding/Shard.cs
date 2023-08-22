using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using MultiunityServer.Socketing;

namespace MultiunityServer.Sharding
{
    internal class Shard
    {
        HashSet<ServerSession> sessions;
        Dictionary<int, Entity> entities;
        Queue<int> tags;
        int spawnedObjectCount;
        public IdGenerator entityIdGenerator;
        public Shard()
        {
            sessions = new HashSet<ServerSession>();
            tags = new Queue<int>();
            entityIdGenerator = new IdGenerator();
        }
        public void AddSession(ServerSession session)
        {
            sessions.Add(session);
            session.shard = this;
            Console.WriteLine("enqued new session");
        }
        public void RemoveSession(ServerSession session)
        {
            sessions.Remove(session);
            session.shard = this;
        }
        public int Occupancy()
        {
            return sessions.Count;
        }
        public void Create(int prefab, Entity entity)
        {
            byte[] entityEncoding = entity.Encoding();
            byte[] encoding = new byte[entityEncoding.Length+2];
            int idx = 0;
            idx = Entity.AppendInt16(encoding, idx, prefab);
            for(int i = idx; i < encoding.Length; i++)
            {
                encoding[i] = entityEncoding[i-idx];
            }
            foreach(ServerSession session in sessions)
            {
                if (session == entity.owner) continue;
                session.Send(encoding);
            }
        }
    }
}
