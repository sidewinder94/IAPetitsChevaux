using System;
using System.Collections.Generic;
using Microsoft.ServiceModel.WebSockets;
using Newtonsoft.Json;

namespace WebSocketsTest
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
                    {String.Format("c-{0}", normalize(i)), 1},
                    {String.Format("c-{0}", normalize(i + 14)), 2},
                    {String.Format("c-{0}", normalize(i + 28)), 3},
                    {String.Format("c-{0}", normalize(i + 42)), 4},
                    {String.Format("e-{0}-{1}", 1, normalize(i, 6, 1)), 0},
                    {String.Format("e-{0}-{1}", 2, normalize(i, 6, 1)), 0},
                    {String.Format("e-{0}-{1}", 3, normalize(i, 6, 1)), 0},
                    {String.Format("e-{0}-{1}", 4, normalize(i, 6, 1)), 0},
                    {String.Format("sq-{0}", 1), normalize(i, 4, 1)},
                    {String.Format("sq-{0}", 2), normalize(i, 4, 1)},
                    {String.Format("sq-{0}", 3), normalize(i, 4, 1)},
                    {String.Format("sq-{0}", 4), normalize(i, 4, 1)}
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

        private int normalize(int i, int against = 56, int @base = 0)
        {
            while (true)
            {
                if (i < (against + @base)) return i;

                if (i >= (against + @base))
                {
                    var t = i - against;
                    if (t < (against + @base)) return t;
                    i = t;
                    continue;
                }

                break;
            }

            throw new ArgumentOutOfRangeException("i", i, "should be > " + @base);
        }


    }
}