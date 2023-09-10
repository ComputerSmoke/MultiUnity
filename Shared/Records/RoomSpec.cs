using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Records
{
    public class RoomSpec
    {
        public int id { get; set; }
        public int shardSize { get; set; }
        public RoomSpec(int id, int shardSize)
        {
            this.id = id;
            this.shardSize = shardSize;
        }
    }
}
