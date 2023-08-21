using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;

namespace MultiunityServer
{
    internal class ServerSession : ISession
    {
        public Socket socket;
        public Queue<Entity> creates;
        Decoder decoder;
        Encoder encoder;
        SocketHandler socketHandler;

        public ServerSession(Socket socket, SocketHandler socketHandler)
        {
            this.socket = socket;
            this.socketHandler = socketHandler;
            creates = new Queue<Entity>();
        }
        public Socket GetSocket()
        {
            return socket;
        }
        
        private int NpcShip()
        {
            newNpcs.Enqueue(ReadObj());
            return 0;
        }
        private int MyShip()
        {
            ship = ReadObj();
            Console.WriteLine("x: " + ship.pos.x + " y: " + ship.pos.y);
            return 0;
        }
        private int Shot()
        {
            newProjectiles.Enqueue(ReadObj());
            return 0;
        }
        private int Entered()
        {
            Area oArea = area;
            Console.WriteLine("entering");
            int areaNum = ReadInt();
            Area tarea = areas.GetArea(areaNum);
            area = tarea;
            tarea.AddSession(this);
            if (oArea != null) oArea.RemoveSession(this);
            return 0;
        }
        private int Died()
        {
            killer = ReadInt();
            return 0;
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
