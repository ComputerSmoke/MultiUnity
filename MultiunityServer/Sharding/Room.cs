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
        public int id;
        int shardSize;
        List<Shard> shards;
        public Room(int id, int shardSize)
        {
            this.shardSize = shardSize;
            this.id = id;
            shards = new List<Shard>();
        }
        public Room(RoomSpec spec)
        {
            id = spec.id;
            shardSize = spec.shardSize;
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
            Shard newShard = new Shard();
            newShard.AddSession(session);
            shards.Add(newShard);
        }
        public RoomSpec Spec()
        {
            return new RoomSpec(id, shardSize);
        }
    }
}
