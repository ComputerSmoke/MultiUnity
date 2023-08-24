using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiunityServer.Sharding
{
    static internal class World
    {
        static Dictionary<int, Room> rooms = new Dictionary<int, Room>();
        public static void AddRoom(Room room)
        {
            rooms[room.id] = room;
        }
        public static void RemoveRoom(Room room)
        {
            rooms.Remove(room.id);
        }
        public static Room GetRoom(int id)
        {
            return rooms[id];
        }
    }
}
