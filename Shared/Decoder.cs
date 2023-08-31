using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Multiunity.Shared
{
    using Vec = Tuple<float, float>;
    public class Decoder
    {
        enum InCode
        {
            JOIN_ROOM,
            CREATE,
            UPDATE,
            DESTROY
        }
        InCode? reading;
        Queue<byte> inQueue;
        byte[] inBuf;
        ISession session;
        Func<int>[] codeFunctions;
        int[] codeLengths;

        public Decoder(ISession session)
        {
            this.session = session;
            inQueue = new Queue<byte>();
            inBuf = new byte[1000];
            codeFunctions = new Func<int>[] { Join, Create, Update, Destroy };
            codeLengths = new int[] { 2, 42, 40, 2 };
            Listen();
        }
        private int Join()
        {
            int room = ReadInt16();
            session.Join(room);
            return 0;
        }
        private int Create()
        {
            int prefab = ReadInt16();
            Entity entity = ReadEntity();
            session.Create(prefab, entity);
            return 0;
        }
        private int Update()
        {
            Entity entity = ReadEntity();
            session.Update(entity);
            return 0;
        }
        private int Destroy()
        {
            int id = ReadInt16();
            session.Destroy(id);
            return 0;
        }
        private Entity ReadEntity()
        {
            int clientId = ReadInt16();
            Vec position = ReadVec();
            Vec velocity = ReadVec();
            Vec accel = ReadVec();
            float rotation = ReadFloat();
            float rotVel = ReadFloat();
            float rotAccel = ReadFloat();
            int parent = ReadInt16();
            return new Entity(clientId, position, velocity, accel, rotation, rotVel, rotAccel, parent);
        }
        private float ReadFloat()
        {
            byte[] bytes = ReadBytes(4);
            return BitConverter.ToSingle(bytes);
        }
        private int ReadInt16()
        {
            byte[] bytes = ReadBytes(2);
            return (((int)(bytes[0])) << 8) | ((int)(bytes[1]));
        }
        private Vec ReadVec()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            return new Vec(x, y);
        }
        private byte[] ReadBytes(int n)
        {
            byte[] bytes = new byte[n];
            for (int i = 0; i < n; i++) bytes[i] = inQueue.Dequeue();
            return bytes;
        }
        private void ReadQueue()
        {
            if (inQueue.Count == 0) return;
            if (reading == null)
            {
                int readNum = inQueue.Dequeue();
                if (readNum < 0 || readNum >= codeLengths.Length) return;
                reading ??= (InCode)readNum;
            }
            Console.WriteLine("reading: " + reading);
            if (inQueue.Count < codeLengths[(int)reading]) return;
            codeFunctions[(int)reading]();
            reading = null;
            ReadQueue();
        }
        private async void Listen()
        {
            for (; ; )
            {
                try
                {
                    await Task.Delay(100);
                    Socket socket = session.GetSocket();
                    if (socket.Available == 0) continue;
                    int received = socket.Receive(inBuf, SocketFlags.None);
                    for (int i = 0; i < received; i++) inQueue.Enqueue(inBuf[i]);
                    ReadQueue();
                }
                catch(Exception ex)
                {
                    inQueue.Clear();
                    reading = null;
                }
            }
        }
    }
}
