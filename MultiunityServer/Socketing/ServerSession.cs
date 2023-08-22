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
        Encoder encoder;
        SocketHandler socketHandler;
        public Shard shard;

        public ServerSession(Socket socket, SocketHandler socketHandler)
        {
            this.socket = socket;
            this.socketHandler = socketHandler;
            creates = new Queue<Entity>();
            this.shard = new();
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


        public void Send(byte[] data)
        {
            try
            {
                socket.Send(data);
            }
            catch
            {
                area.RemoveSession(this);
                socket.Close();
            }
        }
    }
}
