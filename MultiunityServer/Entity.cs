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
        public int parent;
        public int id;
        public Entity(Vec pos, Vec vel, Vec accel, float rot, int parent)
        {
            this.pos = pos;
            this.vel = vel;
            this.accel = accel;
            this.rot = rot;
            this.parent = parent;
        }
    }
}
