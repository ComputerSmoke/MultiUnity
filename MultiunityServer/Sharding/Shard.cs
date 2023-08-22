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
        EntityDictionary entities;
        public Shard()
        {
            sessions = new HashSet<ServerSession>();
            entities = new();
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
            entities.Add(entity);
            byte[] entityEncoding = entity.Encoding();
            byte[] encoding = new byte[entityEncoding.Length+2];
            int idx = 0;
            idx = Entity.AppendInt16(encoding, idx, prefab);
            for(int i = idx; i < encoding.Length; i++)
            {
                encoding[i] = entityEncoding[i-idx];
            }
            ForwardAll(entity.owner, encoding);
        }
        public void Update(Entity entity)
        {
            entities.Add(entity);
            byte[] encoding = entity.Encoding();
            ForwardAll(entity.owner, encoding);
        }
        public void Destroy(ServerSession owner, int clientId)
        {
            Entity entity = entities.Get(owner, clientId);
            byte[] encoding = new byte[2];
            Entity.AppendInt16(encoding, 0, entity.id);
            entities.Destroy(entity);
            ForwardAll(owner, encoding);
        }
        private void ForwardAll(ServerSession excluded, byte[] data)
        {
            foreach (ServerSession session in sessions)
            {
                if (session == excluded) continue;
                session.Send(data);
            }
        }
    }
}
