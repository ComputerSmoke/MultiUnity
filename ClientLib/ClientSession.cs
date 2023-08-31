using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiunity.Shared;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Net;

namespace Multiunity.Client
{
    public class ClientSession : ISession
    {
        Socket socket;
        Func<int, Entity, int> CreateFun;
        Func<Entity, int> UpdateFun;
        Func<int, int> DestroyFun;
        Shared.Decoder decoder;
        public ClientSession(int tcpPort, Func<int, Entity, int> Create, Func<Entity, int> Update, Func<int, int> Destroy) {
            this.CreateFun = Create;
            this.UpdateFun = Update;
            this.DestroyFun = Destroy;
            socket = Connect(tcpPort);
            decoder = new(this);
        }
        private static Socket Connect(int tcpPort)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipEndPoint = new(ipAddress, tcpPort);
            Socket socket = new(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);
            socket.Connect(ipEndPoint);
            return socket;
        }
        public void Join(int roomId) { }
        public void Create(int prefabId, Entity entity)
        {
            CreateFun(prefabId, entity);
        }
        public void Update(Entity entity)
        {
            UpdateFun(entity);
        }
        public void Destroy(int id)
        {
            DestroyFun(id);
        }
        public Socket GetSocket()
        {
            return socket;
        }
        public void Send(byte[] data)
        {
            socket.Send(data);
        }
    }
}
