using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiunityServer
{
    internal class EntityDictionary : ICollection<Entity> {
        Dictionary<int, Entity> entities;
        Dictionary<int, Dictionary<int, Entity>> parentIndex;
        public int Count { get { return entities.Count; } } 
        public bool IsReadOnly { get { return false; } }
        public EntityDictionary()
        {
            entities = new();
            parentIndex = new();
        }
        public void Add(Entity entity)
        {
            Remove(entity);
            entities[entity.id] = entity;
            if (!parentIndex.ContainsKey(entity.parent)) parentIndex[entity.parent] = new();
            parentIndex[entity.parent][entity.id] = entity;
        }
        public bool Remove(Entity entity)
        {
            if (!entities.ContainsKey(entity.id)) return false;
            Entity prevEntry = entities[entity.id];
            entities.Remove(prevEntry.id);
            if (!parentIndex.ContainsKey(prevEntry.parent)) return true;
            var index = parentIndex[prevEntry.parent];
            if (!index.ContainsKey(prevEntry.id)) return true;
            index.Remove(prevEntry.id);
            return true;
        }
        public void Clear()
        {
            entities.Clear();
            parentIndex.Clear();
        }
        public bool Contains(Entity entity)
        {
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
        public System.Collections.IEnumerator GetEnumerator()
        {
            return entities.Values.GetEnumerator();
        }
    }
}
