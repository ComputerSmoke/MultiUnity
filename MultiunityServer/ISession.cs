using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace MultiunityServer
{
    internal interface ISession
    {
        void Join();
        void Create();
        void Update();
        void Destroy();
        public Socket GetSocket();
    }
}
