﻿using System;
using Microsoft.ServiceModel.WebSockets;
using PetitsChevaux.Game;

namespace PetitsChevaux
{
    class Program
    {
        static void Main(string[] args)
        {
            Board.PlayerNumber = 2;
            Board.PawnsPerPlayer = 4;
            Board.GeneratePlayers();
            var host = new WebSocketHost<SimpleEventingService>(new Uri("ws://localhost:10000/"));
            host.AddWebSocketEndpoint();

            host.Open();

            Console.WriteLine("Server initialized");
            Console.ReadLine();
            host.Close();
        }
    }
}
