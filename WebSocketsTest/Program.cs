using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Core;
using Newtonsoft.Json;
using PetitsChevaux.Contracts;
using SuperSocket.SocketBase;
using PetitsChevaux.Game;
using PetitsChevaux.Plans;
using PetitsChevaux.Plans.MiniMax;
using SuperWebSocket;

namespace PetitsChevaux
{
    class Program
    {

        private static int _port = 10000;
        private static Func<Player, List<Player>, int, Contracts.Action> _nextMove = NegaMax.NextMove;

        static void Main(string[] args)
        {


            ProcessArgs(args);

            var appServer = new WebSocketServer();

            if (!appServer.Setup(_port))
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

        private static void ProcessArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-p":
                    case "-P":
                        int tPort;
                        var success = int.TryParse(args[++i], out tPort);
                        if (success)
                        {
                            _port = tPort;
                        }
                        else
                        {
                            Console.WriteLine("Port value can't be parsed : {0}", args[i]);
                        }
                        break;

                    case "--simple":
                        _nextMove = SimpleMinded.NextMove;
                        break;
                    default:
                        Console.WriteLine("Unknown Param : {0}", args[i]);
                        break;
                }
            }
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
