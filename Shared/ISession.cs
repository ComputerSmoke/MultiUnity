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
        void Create(int prefab, Entity entity);
        void Update(Entity entity);
        void Destroy(int id);
        public Socket GetSocket();
    }
}
