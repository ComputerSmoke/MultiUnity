﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using Multiunity.Server.Sharding;

namespace Multiunity.Server.Socketing
{
    internal class SocketHandler
    {
        Socket listener;
        HashSet<ServerSession> sessions;
        World world;
        public SocketHandler(World world)
        {
            this.world = world;
            listener = Init(11_000);
            sessions = new HashSet<ServerSession>();
        }
        public SocketHandler(int port, World world)
        {
            this.world = world;
            listener = Init(port);
            sessions = new HashSet<ServerSession>();
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
                ServerSession session = new ServerSession(handler, world);
                sessions.Add(session);
                Console.WriteLine("Made session");
            }
        }
        public void Destroy(ServerSession session)
        {
            sessions.Remove(session);
        }
    }
}
