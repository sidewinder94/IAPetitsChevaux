using System;
using Microsoft.ServiceModel.WebSockets;

namespace WebSocketsTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebSocketHost<SimpleEventingService>(new Uri("ws://localhost:10000/"));
            host.AddWebSocketEndpoint();

            host.Open();

            Console.WriteLine("Server initialized");
            Console.ReadLine();
            host.Close();
        }
    }
}
