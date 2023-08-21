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
    internal class Session
    {
        enum InCode
        {
            JOIN_ROOM,
            CREATE,
            UPDATE,
            DESTROY
        }
        static int[] codeLengths = { 0,  };
        Func<int>[] codeFunctions;
        public Socket socket;
        public Obj? ship;
        public Queue<Obj> newNpcs;
        public Queue<Obj> newProjectiles;
        public byte tag;
        Queue<byte> inQueue;
        byte[] inBuf;
        public Area area;
        Areas areas;
        public int? killer;

        InCode? reading;

        public Session(Socket socket, Areas areas, Area area)
        {
            this.socket = socket;
            inQueue = new Queue<byte>();
            inBuf = new byte[1000];
            newNpcs = new Queue<Obj>();
            newProjectiles = new Queue<Obj>();
            codeFunctions = new Func<int>[] { NpcShip, MyShip, Shot, Entered, Died };
            this.areas = areas;
            this.area = area;
            tag = area.AddSession(this);
            Listen();
        }
        private Obj ReadObj()
        {
            Vec position = ReadVec();
            Vec velocity = ReadVec();
            float rotation = ReadFloat();
            byte data = inQueue.Dequeue();
            return new Obj(position, velocity, rotation, data);
        }
        private float ReadFloat()
        {
            byte[] bytes = ReadBytes();
            return BitConverter.ToSingle(bytes);
        }
        private int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes());
        }
        private Vec ReadVec()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            return new Vec(x, y);
        }
        private byte[] ReadBytes()
        {
            byte[] bytes = new byte[4];
            for (int i = 0; i < 4; i++) bytes[i] = inQueue.Dequeue();
            return bytes;
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
        private async void Listen()
        {
            for (; ; )
            {
                try
                {
                    await Task.Delay(100);
                    if (socket.Available == 0) continue;
                    int received = socket.Receive(inBuf, SocketFlags.None);
                    for (int i = 0; i < received; i++) inQueue.Enqueue(inBuf[i]);
                    ReadQueue();
                }
                catch
                {
                    inQueue.Clear();
                    reading = null;
                }
            }
        }
        private void ReadQueue()
        {
            if (inQueue.Count == 0) return;
            reading ??= (InCode)inQueue.Dequeue();
            Console.WriteLine("reading: " + reading);
            if (inQueue.Count < codeLengths[(int)reading]) return;
            codeFunctions[(int)reading]();
            reading = null;
            ReadQueue();
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
