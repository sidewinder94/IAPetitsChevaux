using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Core;
using Newtonsoft.Json;
using PetitsChevaux.Contracts;
using SuperSocket.SocketBase;
using PetitsChevaux.Game;
using SuperWebSocket;

namespace PetitsChevaux
{
    class Program
    {
        static void Main(string[] args)
        {

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
            var boardStatus = JsonConvert.DeserializeObject<BoardStatus>(value);

            var board = new Board(boardStatus.State);

            session.Send(JsonConvert.SerializeObject(board.NextTurn(boardStatus.PlayerIdIs, boardStatus.Roll)));

        }

        static void appServer_SessionClosed(WebSocketSession session, CloseReason value)
        {
            Console.WriteLine("Session closed for {0}", value);
        }

        private static void AppServerOnNewSessionConnected(WebSocketSession session)
        {
        }
    }
}
