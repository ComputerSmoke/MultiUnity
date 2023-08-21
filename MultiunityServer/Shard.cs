using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MultiunityServer
{
    internal class Shard
    {
        HashSet<ServerSession> sessions;
        Dictionary<int, Entity> entities;
        Queue<ServerSession> toAdd;
        Queue<ServerSession> toRemove;
        Queue<int> tags;
        int spawnedObjectCount;
        public Shard()
        {
            sessions = new HashSet<ServerSession>();
            toAdd = new();
            toRemove = new();
            tags = new Queue<int>();
        }
        public void AddSession(ServerSession session)
        {
            toAdd.Enqueue(session);
            Console.WriteLine("enqued new session");
        }
        public void RemoveSession(ServerSession session)
        {
            toRemove.Enqueue(session);
        }
        public void Update()
        {
            //possible race condition between areas here if this were made multi-threaded
            while (toAdd.Count > 0)
            {
                ServerSession sess = toAdd.Dequeue();
                if (sess.area == this) sessions.Add(sess);
            }
            while (toRemove.Count > 0)
            {
                ServerSession sess = toRemove.Dequeue();
                if (sess.area != this) sessions.Remove(sess);
            }

            foreach (ServerSession session in sessions)
            {
                if (session == null) continue;
                byte[] bullets = Bullets(session);
                byte[] npcShips = NpcShips(session);
                byte[] ships = Ships(session);
                byte[] data = new byte[bullets.Length + npcShips.Length + ships.Length];
                for (int i = 0; i < bullets.Length; i++) data[i] = bullets[i];
                for (int i = 0; i < npcShips.Length; i++) data[i + bullets.Length] = npcShips[i];
                for (int i = 0; i < ships.Length; i++) data[i + bullets.Length + npcShips.Length] = ships[i];
                session.Send(data);
            }
            foreach (ServerSession session in sessions)
            {
                if (session == null) continue;
                if (session.newNpcs != null) session.newNpcs.Clear();
                if (session.newProjectiles != null) session.newProjectiles.Clear();
            }
        }

        private byte[] Bullets(Session excluded)
        {
            List<Obj> bullets = new List<Obj>();
            foreach (Session session in sessions)
            {
                if (session == excluded || session.newProjectiles == null) continue;
                foreach (Obj bullet in session.newProjectiles) bullets.Add(bullet);
            }
            byte[] data = new byte[22 * bullets.Count];
            for (int i = 0; i < bullets.Count; i++)
            {
                data[22 * i] = (byte)OutCode.BULLET;
                byte[] bytes = EncodeObj(bullets[i]);
                for (int j = 0; j < bytes.Length; j++) data[22 * i + j + 1] = bytes[j];
            }
            return data;
        }
        private byte[] NpcShips(Session excluded)
        {
            List<Obj> ships = new List<Obj>();
            foreach (Session session in sessions)
            {
                if (session == excluded || session.newNpcs == null) continue;
                foreach (Obj ship in session.newNpcs) ships.Add(ship);
            }
            byte[] data = new byte[22 * ships.Count];
            for (int i = 0; i < ships.Count; i++)
            {
                data[22 * i] = (byte)OutCode.NPC;
                byte[] bytes = EncodeObj(ships[i]);
                for (int j = 0; j < bytes.Length; j++) data[22 * i + j + 1] = bytes[j];
            }
            return data;
        }
        private byte[] Ships(Session excluded)
        {
            List<Tuple<byte, Obj>> ships = new List<Tuple<byte, Obj>>();
            foreach (Session session in sessions)
            {
                if (session == excluded || session.ship == null) continue;
                ships.Add(new Tuple<byte, Obj>(session.tag, session.ship));
            }
            byte[] data = new byte[23 * ships.Count];
            for (int i = 0; i < ships.Count; i++)
            {
                byte[] bytes = EncodeObj(ships[i].Item2);
                data[23 * i] = (byte)OutCode.SHIP;
                Console.WriteLine("x: " + ships[i].Item2.pos.x);
                for (int j = 0; j < bytes.Length; j++) data[23 * i + j + 1] = bytes[j];
                data[23 * i + 22] = ships[i].Item1;
            }
            return data;
        }
        private static byte[] EncodeObj(Obj obj)
        {
            byte[] data = new byte[21];
            AppendFloat(data, 0, obj.pos.x);
            AppendFloat(data, 4, obj.pos.y);
            AppendFloat(data, 8, obj.vel.x);
            AppendFloat(data, 12, obj.vel.y);
            AppendFloat(data, 16, obj.rot);
            data[20] = obj.data;
            return data;
        }
        private static void AppendFloat(byte[] buf, int start, float f)
        {
            byte[] bytes = BitConverter.GetBytes(f);
            for (int i = 0; i < 4; i++) buf[start + i] = bytes[i];
        }
    }
}
