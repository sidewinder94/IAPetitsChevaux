using System;
using System.Text.RegularExpressions;
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
            Board board = new Board();

            SimpleEventingService.board = board;

            var host = new WebSocketHost<SimpleEventingService>(new Uri("ws://localhost:10000/"));
            host.AddWebSocketEndpoint();

            host.Open();

            Console.WriteLine("Server initialized");
            Console.ReadLine();
        }
    }
}
