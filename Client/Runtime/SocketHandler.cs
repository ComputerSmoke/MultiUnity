using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public static class SocketHandler
{
    static Socket client;
    public static void Connect() {
        IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint ipEndPoint = new(ipAddress, 11_000);
        client = new(
            ipEndPoint.AddressFamily, 
            SocketType.Stream, 
            ProtocolType.Tcp);
        client.Connect(ipEndPoint);
    }
    
    public static void Send(byte[] data) {
        client.Send(data);
    }
}
