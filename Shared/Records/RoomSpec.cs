using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Records
{
    public class RoomSpec
    {
        public int id { get; set; }
        public int shardSize { get; set; }
        public bool multiShard { get; set; }
        public RoomSpec(int id, int shardSize, bool multiShard)
        {
            this.id = id;
            this.shardSize = shardSize;
            this.multiShard = multiShard;
        }
    }
}
