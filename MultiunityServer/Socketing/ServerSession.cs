using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using Multiunity.Server.Sharding;
using Multiunity.Shared;

namespace Multiunity.Server.Socketing
{
    internal class ServerSession : ISession
    {
        public Socket socket;
        public Queue<Entity> creates;
        Shared.Decoder decoder;
        public Shard shard;
        private World world;

        public ServerSession(Socket socket, World world)
        {
            this.socket = socket;
            creates = new Queue<Entity>();
            this.shard = new();
            this.decoder = new (socket, this);
            this.world = world;
        }
        //TODO: keep backlog of created objects in rooms and send to new clients joining
        public void Join(int roomId)
        {
            world.GetRoom(roomId).Join(this);
        }
        public void Create(int prefab, Entity entity)
        {
            entity.owner = this;
            shard.Create(prefab, entity);
        }
        public void Update(Entity entity)
        {
            entity.owner = this;
            shard.Update(entity);
        }
        public void Destroy(int clientId)
        {
            shard.Destroy(this, clientId);
        }

        public void Send(byte[] data)
        {
            try
            {
                socket.Send(data);
            }
            catch
            {
                shard.RemoveSession(this);
                socket.Close();
            }
        }
    }
}
