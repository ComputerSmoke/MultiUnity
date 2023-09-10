using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Multiunity.Server.Socketing;
using Multiunity.Shared;

namespace Multiunity.Server.Sharding
{
    internal class Shard
    {
        HashSet<ServerSession> sessions;
        EntityDictionary entities;
        public Shard()
        {
            sessions = new HashSet<ServerSession>();
            entities = new EntityDictionary();
        }
        public void AddSession(ServerSession session)
        {
            sessions.Add(session);
            session.shard = this;
            ForwardCreateBacklog(session);
            Console.WriteLine("enqued new session");
        }
        private void ForwardCreateBacklog(ServerSession session)
        {
            foreach(Entity entity in entities)
            {
                session.Send(Encoder.Create(entity));
            }
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
        public void Create(Entity entity)
        {
            entities.Add(entity);
            ForwardAll((ServerSession?)entity.owner, Encoder.Create(entity));
        }
        public void Update(Entity entity)
        {
            entities.Add(entity);
            ForwardAll((ServerSession?)entity.owner, Encoder.Update(entity));
        }
        public void Destroy(ServerSession owner, int clientId)
        {
            Entity entity = entities.Get(owner, clientId);
            //TODO: also destroy children?
            entities.Destroy(entity);
            //Note we forward w/ id, not client id. Make sure this translation is correctly defined and stuff.
            ForwardAll(owner, Encoder.Destroy(entity.id));
        }
        public void Signal(ServerSession owner, int id, byte[] msg)
        {
            ForwardAll(owner, Encoder.Signal(id, msg));
        }
        private void ForwardAll(ServerSession? excluded, byte[] data)
        {
            foreach (ServerSession session in sessions)
            {
                if (session == excluded) continue;
                    session.Send(data);
            }
        }
    }
}
