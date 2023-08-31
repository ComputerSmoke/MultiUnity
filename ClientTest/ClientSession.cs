using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiunity.Shared;

namespace ClientTest
{
    internal class ClientSession : ISession
    {
        public ClientSession() { }
        public void Create(int prefabId, Entity entity)
        {
            Console.WriteLine("Create");
        }
        public void Destroy(int id)
        {
            Console.WriteLine("Destroy");
        }
        public void Update(Entity entity)
        {
            Console.WriteLine("Update");
        }
        public void Join(int roomId)
        {
            Console.WriteLine("Join");
        }
    }
}
