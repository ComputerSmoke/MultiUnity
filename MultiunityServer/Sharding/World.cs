using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiunity.Server.Sharding
{
    internal class World
    {
        Dictionary<int, Room> rooms;
        public string name;
        private bool multiShard;
        private int shardSize;
        public World(int shardSize, bool multiShard)
        {
            rooms = new Dictionary<int, Room>();
            name = "default";
            this.multiShard = multiShard;
            this.shardSize = shardSize;
        }
        public World(string name, int shardSize, bool multiShard)
        {
            rooms = new();
            this.name = name;
            this.multiShard = multiShard;
            this.shardSize = shardSize;
        }
        public void CreateRoom(int id)
        {
            Room room = new(id, shardSize, multiShard);
            rooms[room.id] = room;
        }
        public void RemoveRoom(Room room)
        {
            rooms.Remove(room.id);
        }
        public Room GetRoom(int id)
        {
            return rooms[id];
        }
        public bool HasRoom(int id)
        {
            return rooms.ContainsKey(id);
        }
        public List<Room> RoomList()
        {
            return rooms.Values.ToList();
        }
    }
}
