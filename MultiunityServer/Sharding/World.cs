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
        public World()
        {
            rooms = new Dictionary<int, Room>();
            name = "default";
        }
        public World(string name)
        {
            rooms = new();
            this.name = name;
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
