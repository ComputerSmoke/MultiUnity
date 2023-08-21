using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace MultiunityServer
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
                var handler = await newListener.AcceptAsync();
                new Session(handler, sessions);
                Console.WriteLine("Made session");
            }
        }
    }
}
