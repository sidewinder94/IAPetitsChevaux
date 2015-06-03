using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.ServiceModel.WebSockets;
using Newtonsoft.Json;
using PetitsChevaux.Contract;
using PetitsChevaux.Game;


namespace PetitsChevaux
{
    public class SimpleEventingService : WebSocketService
    {
        public static Board board = null;

        public static Boolean Run = true;
        #region Overrides of WebSocketService

        public override void OnOpen()
        {
            board.GeneratePlayers();
        }


        public override void OnMessage(string message)
        {
            try
            {
                var rec = JsonConvert.DeserializeObject<Received>(message);
                if (board.PlayerId == -1) board.PlayerId = rec.PlayerIdIs;
                Console.WriteLine("PLaying for player {0}, asked {1}", board.PlayerId, rec.PlayerIdIs);
                Player player;
                rec.Players.ForEach(pl =>
                {
                    player = board.Players.First(p => p.Id == pl.Id);
                    player.Pawns.Clear();
                    player.Pawns.AddRange(pl.Pawns);

                });
                Send(JsonConvert.SerializeObject(board.NextTurn(rec.Roll == 0 ? -1 : rec.Roll)));
            }
            catch (Exception)
            {
                Send(JsonConvert.SerializeObject(new Send { Players = board.Players }));
            }
        }


        protected override void OnClose()
        {
            Run = false;
        }

        protected override void OnError()
        {
            Run = false;
        }

        #endregion

    }
}