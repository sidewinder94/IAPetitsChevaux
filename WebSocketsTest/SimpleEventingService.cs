using System;
using System.Collections.Generic;
using Microsoft.ServiceModel.WebSockets;
using Newtonsoft.Json;
using PetitsChevaux.Game;


namespace PetitsChevaux
{
    public class SimpleEventingService : WebSocketService
    {

        public static Boolean run = true;
        #region Overrides of WebSocketService

        public override void OnOpen()
        {
            int i = 0;
            while (run)
            {
                var dic = new Dictionary<String, int>
                {
                
                    {String.Format("c-{0}", Board.Normalize(i)), 1},
                    {String.Format("c-{0}", Board.Normalize(i + 14)), 2},
                    {String.Format("c-{0}", Board.Normalize(i + 28)), 3},
                    {String.Format("c-{0}", Board.Normalize(i + 42)), 4},
                    {String.Format("e-{0}-{1}", 1, Board.Normalize(i, 6, 1)), 0},
                    {String.Format("e-{0}-{1}", 2, Board.Normalize(i, 6, 1)), 0},
                    {String.Format("e-{0}-{1}", 3, Board.Normalize(i, 6, 1)), 0},
                    {String.Format("e-{0}-{1}", 4, Board.Normalize(i, 6, 1)), 0},
                    {String.Format("sq-{0}", 1), Board.Normalize(i, 4, 1)},
                    {String.Format("sq-{0}", 2), Board.Normalize(i, 4, 1)},
                    {String.Format("sq-{0}", 3), Board.Normalize(i, 4, 1)},
                    {String.Format("sq-{0}", 4), Board.Normalize(i, 4, 1)}
                };
                Send(JsonConvert.SerializeObject(dic));
                i++;
                System.Threading.Thread.Sleep(1000);
            }
        }


        protected override void OnClose()
        {
            run = false;
        }

        protected override void OnError()
        {
            run = false;
        }

        #endregion

    }
}