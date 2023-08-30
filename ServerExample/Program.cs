using MultiunityServer;

Server server = new(11_000);

server.CreateRoom(10, 1);

for (; ; ) { }