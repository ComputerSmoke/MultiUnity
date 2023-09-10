using Multiunity.Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Multiunity.Client
{
    public class Client
    {
        Socket socket;
        Shared.Decoder decoder;
        public Client(int tcpPort, ISession session)
        {
            socket = Connect(tcpPort);
            decoder = new Shared.Decoder(socket, session);
        }
        private static Socket Connect(int tcpPort)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, tcpPort);
            Socket socket = new Socket(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);
            socket.Connect(ipEndPoint);
            return socket;
        }
        public void Create(int prefabId, Entity entity)
        {
            entity.prefabId = prefabId;
            Send(Shared.Encoder.Create(entity));
        }
        public void Update(Entity entity)
        {
            Send(Shared.Encoder.Update(entity));
        }
        public void Join(int roomId)
        {
            Send(Shared.Encoder.Join(roomId));
        }
        public void Destroy(int id)
        {
            Send(Shared.Encoder.Destroy(id));
        }
        public void Signal(int id, byte[] msg)
        {
            Send(Shared.Encoder.Signal(id, msg));
        }
        private void Send(byte[] data)
        {
            socket.Send(data);
        }
    }
}
