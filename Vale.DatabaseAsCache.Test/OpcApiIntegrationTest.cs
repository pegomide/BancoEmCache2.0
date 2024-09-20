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
        private readonly OpcApiInterface _api = new OpcApiInterface(new OpcApiOptions("http://localhost:3002/api/opc/", "localhost", "Schneider-Aut.OFS.2"));

        [TestMethod]
        public void PostTemNovoRegistro_Test()
        {
            string response = _api.PostVerificaNovoRegistro();
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void PostDataNorth_Test()
        {
            string response = _api.PostDataFromPier();
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void PostEnviaWatchDog()
        {
            bool response = _api.PostSendWatdogSignal(true);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void PostEnviaConfirmação()
        {
            bool response = _api.PostSendConfirmationSignal(true);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void PostEnviaSetGPVDelay()
        {
            bool response = _api.PostSendWatdogSignal(true);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void PostEnviaResetGPVDelay()
        {
            bool response = _api.PostSendWatdogSignal(false);
            Assert.IsNotNull(response);
        }
    }
}
