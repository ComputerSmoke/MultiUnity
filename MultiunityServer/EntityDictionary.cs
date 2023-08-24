﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiunityServer.Socketing;

namespace MultiunityServer
{
    internal class EntityDictionary : ICollection<Entity> {
        Dictionary<int, Entity> entities;
        Dictionary<int, Dictionary<int, Entity>> parentIndex;
        public int Count { get { return entities.Count; } } 
        public bool IsReadOnly { get { return false; } }
        IdGenerator<(ServerSession, int)> entityIdGenerator;
        public EntityDictionary()
        {
            entities = new();
            parentIndex = new();
            entityIdGenerator = new();
        }
        private void AssignId(Entity entity)
        {
            entity.id = entityIdGenerator.Assign((entity.owner, entity.clientId));
        }
        public void Add(Entity entity)
        {
            AssignId(entity);
            Remove(entity);
            entities[entity.id] = entity;
            if (!parentIndex.ContainsKey(entity.parent)) parentIndex[entity.parent] = new();
            parentIndex[entity.parent][entity.id] = entity;
        }
        public bool Remove(Entity entity)
        {
            AssignId(entity);
            if (!entities.ContainsKey(entity.id)) return false;
            Entity prevEntry = entities[entity.id];
            entities.Remove(prevEntry.id);
            if (!parentIndex.ContainsKey(prevEntry.parent)) return true;
            var index = parentIndex[prevEntry.parent];
            if (!index.ContainsKey(prevEntry.id)) return true;
            index.Remove(prevEntry.id);
            return true;
        }
        public void Destroy(Entity entity)
        {
            entityIdGenerator.Release(entity.id);
            Remove(entity);
        }
        public void Destroy(ServerSession session, int clientId)
        {
            Destroy(entities[entityIdGenerator.GetId((session, clientId))]);
        }
        public Entity Get(int id)
        {
            return entities[id];
        }
        public Entity Get(ServerSession session, int clientId)
        {
            return Get(entityIdGenerator.GetId((session, clientId)));
        }
        public Dictionary<int, Entity> Children(Entity parent)
        {
            AssignId(parent);
            if (!parentIndex.ContainsKey(parent.id)) return new();
            return parentIndex[parent.id];
        }
        public void Clear()
        {
            entities.Clear();
            parentIndex.Clear();
        }
        public bool Contains(Entity entity)
        {
            AssignId(entity);
            return entities.ContainsKey(entity.id);
        }
        public void CopyTo(Entity[] arr, int start)
        {
            entities.Values.CopyTo(arr, start);
        }
        public IEnumerator<Entity> GetEnumerator()
        {
            return entities.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return entities.Values.GetEnumerator();
        }
    }
}
