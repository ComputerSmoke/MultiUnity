﻿using System;
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
        public Server(int portnum)
        {
            rnd = new();
            world = new();
            handler = new(portnum, world);
        }
        public int CreateRoom(int shardSize, int id)
        {
            Room room = new(id, shardSize);
            world.AddRoom(room);
            return id;
        }
        public int CreateRoom(int shardSize)
        {
            int id = rnd.Next();
            return CreateRoom(shardSize, id);
        }
    }
}
