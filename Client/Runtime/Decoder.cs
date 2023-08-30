using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public static class Decoder 
{
    public struct Entity {
        public int id;
        public Vector3 pos;
        public Vector3 vel;
        public Vector3 accel;
        public float rot;
        public float rotV;
        public float rotA;
        public int parent;
        public Entity(int id, Vector3 pos, Vector3 vel, Vector3 accel, float rot, float rotV, float rotA, int parent) {
            this.id = id;
            this.pos = pos;
            this.vel = vel;
            this.accel = accel;
            this.rot = rot;
            this.rotV = rotV;
            this.rotA = rotA;
            this.parent = parent;
        }
    }
    static Encoder.NetCodes? reading;
    static Queue<byte> inQueue = new Queue<byte>();
    static byte[] inBuf = new byte[1000];
    static Func<int>[] codeFunctions = new Func<int>[] { Create, Update, Destroy };
    static int[] codeLengths = new int[] { 42, 40, 2 };
    private static int Create()
    {
        int prefab = ReadInt16();
        Entity entity = ReadEntity();
        PeerHandler.Create(prefab, entity);
        return 0;
    }
    private static int Update()
    {
        Entity entity = ReadEntity();
        PeerHandler.Update(entity);
        return 0;
    }
    private static int Destroy()
    {
        int id = ReadInt16();
        PeerHandler.Destroy(id);
        return 0;
    }
    private static Entity ReadEntity()
        {
            int clientId = ReadInt16();
            Vector3 position = ReadVec();
            Vector3 velocity = ReadVec();
            Vector3 accel = ReadVec();
            float rotation = ReadFloat();
            float rotVel = ReadFloat();
            float rotAccel = ReadFloat();
            int parent = ReadInt16();
            return new Entity(clientId, position, velocity, accel, rotation, rotVel, rotAccel, parent);
        }
    private static float ReadFloat()
    {
        byte[] bytes = ReadBytes(4);
        return BitConverter.ToSingle(bytes);
    }
    private static int ReadInt16()
    {
        return BitConverter.ToInt32(ReadBytes(2));
    }
    private static Vector3 ReadVec()
    {
        float x = ReadFloat();
        float y = ReadFloat();
        return new Vector3(x, y, 0);
    }
    private static byte[] ReadBytes(int n)
    {
        byte[] bytes = new byte[n];
        for (int i = 0; i < n; i++) bytes[i] = inQueue.Dequeue();
        return bytes;
    }
    private static void ReadQueue()
    {
        if (inQueue.Count == 0) return;
        if (reading == null)
        {
            int readNum = inQueue.Dequeue();
            if (readNum < 0 || readNum >= codeLengths.Length) return;
            reading ??= (Encoder.NetCodes)readNum;
        }
        Console.WriteLine("reading: " + reading);
        if (inQueue.Count < codeLengths[(int)reading]) return;
        codeFunctions[(int)reading]();
        reading = null;
        ReadQueue();
    }
    public static async void Listen()
    {
        for (; ; )
        {
            try
            {
                await Task.Delay(100);
                Socket socket = SocketHandler.client;
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
}
