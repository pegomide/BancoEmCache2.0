using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using Vale.DatabaseAsCache.Service.Infrastructure;

namespace Vale.DatabaseAsCache.Test
{
    [TestClass]
    public class OpcApiUnitTest
    {
        private readonly string basePath = "http://localhost:3002";

        [TestMethod]
        public void IsFalse_ExtraiRespostaTemNovoRegistroTest_Sucess()
        {
            var api = new OpcApi(null, basePath);

            string input = "[{\"Value\":false,\"TimeStamp\":\"2023-04-18T15:35:28.319-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_NEW_INCR\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"False\"}]";
            bool retorno = api.ExtraiRespostaTemNovoRegistro(input);
            Assert.IsFalse(retorno);
        }

        [TestMethod]
        public void IsTrue_ExtraiRespostaTemNovoRegistroTest_Sucess()
        {
            var api = new OpcApi(null, basePath);

            string input = "[{\"Value\":true,\"TimeStamp\":\"2023-04-18T15:35:28.319-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_NEW_INCR\",\"FloatValue\":1,\"DoubleValue\":1,\"Int32Value\":1,\"StringValue\":\"True\"}]";
            bool retorno = api.ExtraiRespostaTemNovoRegistro(input);
            Assert.IsTrue(retorno);
        }

        [TestMethod]
        public void JsonSerializationException_ExtraiRespostaTemNovoRegistroTest_Fail()
        {
            var api = new OpcApi(null, basePath);

            string input = "{\"Value\":true,\"TimeStamp\":\"2023-04-18T15:35:28.319-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_NEW_INCR\",\"FloatValue\":1,\"DoubleValue\":1,\"Int32Value\":1,\"StringValue\":\"True\"}";
            Assert.ThrowsException<JsonSerializationException>(() => api.ExtraiRespostaTemNovoRegistro(input));
        }

        [TestMethod]
        public void InvalidCastException_ExtraiRespostaTemNovoRegistroTest_Fail()
        {
            var api = new OpcApi(null, basePath);

            string input = "[{}]";
            Assert.ThrowsException<InvalidCastException>(() => api.ExtraiRespostaTemNovoRegistro(input));
        }
    }
}
