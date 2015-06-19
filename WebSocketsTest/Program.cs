using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SuperSocket.SocketBase;
using PetitsChevaux.Game;
using SuperWebSocket;

namespace PetitsChevaux
{
    class Program
    {

        private static readonly Dictionary<WebSocketSession, Board> _sessions =
            new Dictionary<WebSocketSession, Board>();
        static void Main(string[] args)
        {

            Board.PlayerNumber = 2;
            Board.PawnsPerPlayer = 4;

            var appServer = new WebSocketServer();

            if (!appServer.Setup(10000))
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            appServer.NewSessionConnected += AppServerOnNewSessionConnected;
            appServer.SessionClosed += appServer_SessionClosed;
            appServer.NewMessageReceived += appServer_NewMessageReceived;

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }



            Console.WriteLine("Server initialized");
            Console.ReadLine();
        }

        private static void appServer_NewMessageReceived(WebSocketSession session, string value)
        {
            var board = _sessions[session];
            //if (board.Players.Any(p => p.Won))
            //{
            //    board.GeneratePlayers();
            //}

            //int i = 100;
            //board.Run = true;
            //while (board.Run)
            //{
            //    session.Send(JsonConvert.SerializeObject(board.NextTurn()));

            //    if (board.Players.Any(p => p.Won))
            //    {
            //        board.Run = false;
            //        Console.WriteLine("{0} won !", board.Players.First(p => p.Won));
            //    }

            //    System.Threading.Thread.Sleep(i);
            //}
        }

        static void appServer_SessionClosed(WebSocketSession session, CloseReason value)
        {
            _sessions.Remove(session);
            Console.WriteLine("Session closed for {0}", value);
        }

        private static void AppServerOnNewSessionConnected(WebSocketSession session)
        {
            _sessions.Add(session, new Board());
            var board = _sessions[session];
            if (board.Players.Any(p => p.Won))
            {
                board.GeneratePlayers();
            }

            int i = 100;
            board.Run = true;
            while (board.Run)
            {
                session.Send(JsonConvert.SerializeObject(board.NextTurn()));

                if (board.Players.Any(p => p.Won))
                {
                    board.Run = false;
                    Console.WriteLine("{0} won !", board.Players.First(p => p.Won));
                }

                System.Threading.Thread.Sleep(i);
            }
        }
    }
}
