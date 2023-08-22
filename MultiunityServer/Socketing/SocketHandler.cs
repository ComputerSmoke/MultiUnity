using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace MultiunityServer.Socketing
{
    internal class SocketHandler
    {
        Socket listener;
        HashSet<Session> sessions;
        public SocketHandler()
        {
            listener = Init(11_000);
            sessions = new HashSet<Session>();
        }
        public SocketHandler(int port)
        {
            listener = Init(port);
            sessions = new HashSet<Session>();
        }
        private Socket Init(int port)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipEndPoint = new(ipAddress, port);

            Socket newListener = new(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            newListener.Bind(ipEndPoint);
            newListener.Listen(100);
            AcceptIncomingSockets(newListener);
            return newListener;
        }

        async void AcceptIncomingSockets(Socket newListener)
        {
            for (; ; )
            {
                Socket handler = await newListener.AcceptAsync();
                Session session = new Session(handler, this);
                sessions.Add(session);
                Console.WriteLine("Made session");
            }
        }
        public void Destroy(Session session)
        {
            sessions.Remove(session);
        }
    }
}
