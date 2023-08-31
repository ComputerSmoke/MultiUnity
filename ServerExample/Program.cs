using Multiunity.Server;

Server server = new(11_000);

server.CreateRoom(10, 1);

for (; ; ) { }