using Multiunity.Server.Socketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Records;

namespace Multiunity.Server.Sharding
{
    internal class Room
    {
        public bool multiShard;
        public int id;
        int shardSize;
        List<Shard> shards;
        public Room(int id, int shardSize, bool multiShard)
        {
            this.shardSize = shardSize;
            this.id = id;
            this.multiShard = multiShard;
            shards = new List<Shard>();
        }
        public Room(RoomSpec spec)
        {
            id = spec.id;
            shardSize = spec.shardSize;
            multiShard = spec.multiShard;
            shards = new();
        }
        public void Join(ServerSession session)
        {
            foreach(Shard shard in shards)
            {
                if (shard.Occupancy() >= shardSize) continue;
                shard.AddSession(session);
                return;
            }
            if (!multiShard && shards.Count > 0) return;
            Shard newShard = new Shard();
            newShard.AddSession(session);
            shards.Add(newShard);

        }
        public RoomSpec Spec()
        {
            return new RoomSpec(id, shardSize, multiShard);
        }
    }
}
