using System;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PetitsChevaux.Contract;
using PetitsChevaux.Game;

namespace TestsPetitsChevaux
{
    [TestClass]
    public class DeserializationTest
    {
        [TestMethod]
        public void TestDeserialize()
        {
            String json =
                "{\"Players\":[{\"Id\":0,\"Pawns\":[{\"TypeConverter\":\"c-{0}\",\"Position\":50},{\"TypeConverter\":\"sq-{0}\",\"Position\":0},{\"TypeConverter\":\"sq-{0}\",\"Position\":0},{\"TypeConverter\":\"sq-{0}\",\"Position\":0}],\"Won\":false,\"StartCase\":0,\"Evaluate\":0},{\"Id\":1,\"Pawns\":[{\"TypeConverter\":\"sq-{0}\",\"Position\":0},{\"TypeConverter\":\"sq-{0}\",\"Position\":0},{\"TypeConverter\":\"sq-{0}\",\"Position\":0},{\"TypeConverter\":\"sq-{0}\",\"Position\":0}],\"Won\":false,\"StartCase\":14,\"Evaluate\":0}]}";

            var rec = JsonConvert.DeserializeObject<Received>(json);

            var p = rec.Players.First(pl => pl.Id == 0).Pawns[0];

            Assert.IsTrue(p.Type == CaseType.Classic && p.Position == 50);

        }
    }
}
