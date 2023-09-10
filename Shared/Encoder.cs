using System;
using System.Collections.Generic;
using System.Text;

namespace Multiunity.Shared
{
    public static class Encoder
    {
        public static byte[] Join(int roomId)
        {
            List<byte> result = new List<byte>();
            result.Add((byte)Decoder.InCode.JOIN_ROOM);
            AppendInt16(result, roomId);
            return result.ToArray();
        }
        public static byte[] Create(Entity entity)
        {
            List<byte> result = new List<byte>();
            result.Add((byte)Decoder.InCode.CREATE);
            AppendInt16(result, entity.prefabId);
            result.AddRange(EncodeEntity(entity));
            return result.ToArray();
        }
        public static byte[] Update(Entity entity)
        {
            List<byte> result = new List<byte>();
            result.Add((byte)Decoder.InCode.UPDATE);
            result.AddRange(EncodeEntity(entity));
            return result.ToArray();
        }
        public static byte[] Destroy(int id)
        {
            List<byte> result = new List<byte>();
            result.Add((byte)Decoder.InCode.JOIN_ROOM);
            AppendInt16(result, id);
            return result.ToArray();
        }
        public static byte[] Signal(int prefabId, byte[] msg)
        {
            List<byte> data = new List<byte>();
            data.Add((byte)Decoder.InCode.SIGNAL);
            AppendInt16(data, prefabId);
            AppendInt16(data, msg.Length);
            foreach(byte b in msg)
                data.Add(b);
            return data.ToArray();
        }
        public static List<byte> EncodeEntity(Entity entity)
        {
            List<byte> data = new List<byte>();
            AppendInt16(data, entity.id);
            AppendVec(data, entity.pos);
            AppendVec(data, entity.vel);
            AppendVec(data, entity.accel);
            AppendFloat(data, entity.rot);
            AppendFloat(data, entity.rotv);
            AppendFloat(data, entity.rota);
            AppendInt16(data, entity.parent);
            return data;
        }
        static void AppendVec(List<byte> buf, (float, float) v)
        {
            AppendFloat(buf, v.Item1);
            AppendFloat(buf, v.Item2);
        }
        static void AppendFloat(List<byte> buf, float f)
        {
            byte[] bytes = BitConverter.GetBytes(f);
            for (int i = 0; i < 4; i++) 
                buf.Add(bytes[i]);
        }
        static void AppendInt16(List<byte> buf, int n)
        {
            buf.Add((byte)((n >> 8) & 0xFF));
            buf.Add((byte)(n & 0xFF));
        }
        public static int[] CodeLengths()
        {
            Entity dummy = new Entity(0, (0f, 0f), (0f, 0f), (0f, 0f), 0f, 0f, 0f, 0);
            int join = Join(0).Length;
            int create = Create(dummy).Length;
            int update = Update(dummy).Length;
            int destroy = Destroy(0).Length;
            return new int[] { join, create, update, destroy };
        }
    }
}
