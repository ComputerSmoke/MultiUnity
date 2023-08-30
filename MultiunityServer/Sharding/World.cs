using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiunityServer.Sharding
{
    internal class World
    {
        Dictionary<int, Room> rooms;
        public World()
        {
            rooms = new Dictionary<int, Room>();
        }
        public void AddRoom(Room room)
        {
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
    }
}
