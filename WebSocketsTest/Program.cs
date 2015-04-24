using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceModel.WebSockets;
using Newtonsoft.Json;

namespace WebSocketsTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebSocketHost<SimpleEventingService>(new Uri("ws://localhost:10000/"));
            var endpoint = host.AddWebSocketEndpoint();

            host.Open();

            Console.WriteLine("Server initialized");
            Console.ReadLine();
            host.Close();
        }
    }
}
