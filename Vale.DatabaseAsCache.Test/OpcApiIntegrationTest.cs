using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vale.DatabaseAsCache.Service;
using Vale.DatabaseAsCache.Service.Infrastructure;

namespace Vale.DatabaseAsCache.Test
{
    // Usage toguether with Mockoon API
    // Configuration in DatabaseAsCache.json on root folder
    // Possible upgrade is include mock of .net framework instead of Mockoon API
    [TestClass]
    public class OpcApiIntegrationTest
    {
        private readonly OpcApiInterface _api = new OpcApiInterface(new OpcApiOptions("http://localhost:3002/api/opc/read", "localhost", "Schneider-Aut.OFS.2"));

        [TestMethod]
        public void PostTemNovoRegistro_Test()
        {
            var response = _api.PostVerificaNovoRegistro();
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void PostVerificaPier_Test()
        {
            var response = _api.PostVerificaPier();
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void PostDataSouth_Test()
        {
            var response = _api.PostDataSouth();
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void PostDataNorth_Test()
        {
            var response = _api.PostDataFromPier();
            Assert.IsNotNull(response);
        }
    }
}
