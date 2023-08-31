using ClientTest;
using Multiunity.Client;
using Multiunity.Shared;
using System;
using System.Threading;
using Multiunity.Server;

public class Test
{
    private static Entity blankEntity()
    {
        return new Entity(0, (0f, 0f), (0f, 0f), (0f, 0f), 0f, 0f, 0f, 0);
    }
    public static void Main(string[] args)
    {
        Server server = new(11_000);

        server.CreateRoom(10, 1);

        Client client1 = new Client(11_000, new ClientSession());
        Client client2 = new Client(11_000, new ClientSession());

        client1.Join(1);
        Thread.Sleep(1000);
        client1.Create(1, blankEntity());
        Thread.Sleep(1000);
        client2.Join(1);
        Thread.Sleep(1000);
        client2.Create(1, blankEntity());
        for (; ; ) { }
    }
}