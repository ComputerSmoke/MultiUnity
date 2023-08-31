using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Multiunity.Shared
{
    public class Entity
    {
        public (float,float) pos;
        public (float, float) vel;
        public (float, float) accel;
        public float rot;
        public float rotv;
        public float rota;
        public int parent;
        public int id;
        public int clientId;
        public ISession owner;
        public Entity(int clientId, (float, float) pos, (float, float) vel, (float, float) accel, 
            float rot, float rotVel, float rotAccel, int parent)
        {
            this.pos = pos;
            this.vel = vel;
            this.accel = accel;
            this.rot = rot;
            this.parent = parent;
            this.clientId = clientId;
            rotv = rotVel;
            rota = rotAccel;
        }
    }
}
