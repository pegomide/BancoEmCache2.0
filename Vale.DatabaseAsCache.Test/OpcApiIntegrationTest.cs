using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Vale.DatabaseAsCache.Service.Infrastructure;

namespace Vale.DatabaseAsCache.Test
{
    [TestClass]
    public class OpcApiIntegrationTest
    {
        [TestMethod]
        public void TemNovoRegistroTest()
        {
            OpcApi api = new OpcApi(null, "http://localhost:3002/api/opc/read");

            string hostname = "localhost";
            string serverName = "Schneider-Aut.OFS.2";
            string tag = "EMB!PIMS_AM_NEW_INCR";
            var response = api.TemNovoRegistro(hostname, serverName, tag);
            Assert.IsNotNull(response);
        }
    }
}
