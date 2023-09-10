using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Numerics;

namespace Multiunity.Shared
{
    public interface ISession
    {
        void Join(int room);
        void Create(Entity entity);
        void Update(Entity entity);
        void Destroy(int id);
        void Signal(int id, byte[] msg);
    }
}
