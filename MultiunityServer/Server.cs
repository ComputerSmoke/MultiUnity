using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiunity.Server.Sharding;
using Multiunity.Server.Socketing;

namespace Multiunity.Server
{
    public class Server
    {
        SocketHandler handler;
        World world;
        Random rnd;
        public Server(int portnum, int shardSize, bool multiShard)
        {
            rnd = new();
            world = new(shardSize, multiShard);
            handler = new(portnum, world);
        }
        public int CreateRoom(int id)
        {
            world.CreateRoom(id);
            return id;
        }
        public int CreateRoom()
        {
            int id = rnd.Next();
            return CreateRoom(id);
        }
    }
}
