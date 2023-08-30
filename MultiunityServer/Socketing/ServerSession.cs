using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using MultiunityServer.Sharding;

namespace MultiunityServer.Socketing
{
    internal class ServerSession : ISession
    {
        public Socket socket;
        public Queue<Entity> creates;
        Decoder decoder;
        public Shard shard;

        public ServerSession(Socket socket)
        {
            this.socket = socket;
            creates = new Queue<Entity>();
            this.shard = new();
            this.decoder = new Decoder(this);
        }
        public Socket GetSocket()
        {
            return socket;
        }
        public void Join(int roomId)
        {
            World.GetRoom(roomId).Join(this);
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
