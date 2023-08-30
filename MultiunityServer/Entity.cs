using MultiunityServer.Socketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MultiunityServer
{
    using Vec = Tuple<float, float>;
    internal class Entity
    {
        Vec pos;
        Vec vel;
        Vec accel;
        float rot;
        float rotv;
        float rota;
        public int parent;
        public int id;
        public float timestamp;
        public int clientId;
        public ServerSession? owner;
        public Entity(int clientId, Vec pos, Vec vel, Vec accel, float rot, float rotVel, float rotAccel, int parent)
        {
            this.timestamp = timestamp;
            this.pos = pos;
            this.vel = vel;
            this.accel = accel;
            this.rot = rot;
            this.parent = parent;
            this.clientId = clientId;
            rotv = rotVel;
            rota = rotAccel;
        }

        public byte[] Encoding()
        {
            byte[] data = new byte[40];
            int i = 0;
            i = AppendVec(data, i, pos);
            i = AppendVec(data, i, vel);
            i = AppendVec(data, i, accel);
            i = AppendFloat(data, i, rot);
            i = AppendFloat(data, i, rotv);
            i = AppendFloat(data, i, rota);
            AppendInt16(data, i, parent);
            return data;
        }
        static int AppendVec(byte[] buf, int start, Vec v)
        {
            start = AppendFloat(buf, start, v.Item1);
            start = AppendFloat(buf, start, v.Item2);
            return start;
        }
        static int AppendFloat(byte[] buf, int start, float f)
        {
            byte[] bytes = BitConverter.GetBytes(f);
            for (int i = 0; i < 4; i++) buf[start + i] = bytes[i];
            return start + 4;
        }
        public static int AppendInt16(byte[] buf, int start, int n)
        {
            byte[] bytes = BitConverter.GetBytes(n);
            for (int i = 0; i < 2; i++) buf[start + i] = bytes[i];
            return start + 2;
        }

    }
}
