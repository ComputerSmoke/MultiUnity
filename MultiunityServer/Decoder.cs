using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiunityServer
{
    using Vec = Tuple<float, float>;
    internal class Decoder
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

        static int[] codeLengths = { 0, };
        Func<int>[] codeFunctions;

        public Decoder(ISession session)
            {
                this.session = session;
                inQueue = new Queue<byte>();
                inBuf = new byte[1000];
        }
        private Entity ReadEntity()
        {
            Vec position = ReadVec();
            Vec velocity = ReadVec();
            Vec accel = ReadVec();
            float rotation = ReadFloat();
            int parent = ReadInt16();
            return new Entity(position, velocity, accel, rotation, parent);
        }
        private float ReadFloat()
        {
            byte[] bytes = ReadBytes(4);
            return BitConverter.ToSingle(bytes);
        }
        private int ReadInt16()
        {
            return BitConverter.ToInt32(ReadBytes(2));
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
    }
}
