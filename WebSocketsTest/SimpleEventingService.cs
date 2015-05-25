using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.ServiceModel.WebSockets;
using Newtonsoft.Json;
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
            if (board.Players.Any(p => p.Won))
            {
                board.GeneratePlayers();
            }

            int i = 100;
            Run = true;
            while (Run)
            {
                ;


                //var dic = new Dictionary<String, int>
                //{

                //    {String.Format("c-{0}", Board.Normalize(i)), 1},
                //    {String.Format("c-{0}", Board.Normalize(i + 14)), 2},
                //    {String.Format("c-{0}", Board.Normalize(i + 28)), 3},
                //    {String.Format("c-{0}", Board.Normalize(i + 42)), 4},
                //    {String.Format("e-{0}-{1}", 1, Board.Normalize(i, 6, 1)), 0},
                //    {String.Format("e-{0}-{1}", 2, Board.Normalize(i, 6, 1)), 0},
                //    {String.Format("e-{0}-{1}", 3, Board.Normalize(i, 6, 1)), 0},
                //    {String.Format("e-{0}-{1}", 4, Board.Normalize(i, 6, 1)), 0},
                //    {String.Format("sq-{0}", 1), Board.Normalize(i, 4, 1)},
                //    {String.Format("sq-{0}", 2), Board.Normalize(i, 4, 1)},
                //    {String.Format("sq-{0}", 3), Board.Normalize(i, 4, 1)},
                //    {String.Format("sq-{0}", 4), Board.Normalize(i, 4, 1)}
                //};
                Send(JsonConvert.SerializeObject(board.NextTurn()));

                if (board.Players.Any(p => p.Won))
                {
                    Run = false;
                    Console.WriteLine("{0} won !", board.Players.First(p => p.Won));
                }

                System.Threading.Thread.Sleep(i);
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