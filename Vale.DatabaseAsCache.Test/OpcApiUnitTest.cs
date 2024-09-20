using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Vale.DatabaseAsCache.Data.TableModels;
using Vale.GetFuseData.ApiService.Services;

namespace Vale.DatabaseAsCache.Test
{
    [TestClass]
    public class OpcApiUnitTest
    {
        [TestMethod]
        public void IsFalse_ExtraiRespostaTemNovoRegistroTest_Sucess()
        {
            string input = "[{\"Value\":false,\"TimeStamp\":\"2023-04-18T15:35:28.319-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_AM_NEW_INCR_TX\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"False\"}]";
            bool retorno = OpcApiService.ConverteNovoRegistro(input);
            Assert.IsFalse(retorno);
        }

        [TestMethod]
        public void IsTrue_ExtraiRespostaTemNovoRegistroTest_Sucess()
        {
            string input = "[{\"Value\":true,\"TimeStamp\":\"2023-04-18T15:35:28.319-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_AM_NEW_INCR_TX\",\"FloatValue\":1,\"DoubleValue\":1,\"Int32Value\":1,\"StringValue\":\"True\"}]";
            bool retorno = OpcApiService.ConverteNovoRegistro(input);
            Assert.IsTrue(retorno);
        }

        [TestMethod]
        public void JsonSerializationException_ExtraiRespostaTemNovoRegistroTest_Fail()
        {
            string input = "{\"Value\":true,\"TimeStamp\":\"2023-04-18T15:35:28.319-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_AM_NEW_INCR_TX\",\"FloatValue\":1,\"DoubleValue\":1,\"Int32Value\":1,\"StringValue\":\"True\"}";
            Assert.ThrowsException<JsonSerializationException>(() => OpcApiService.ConverteNovoRegistro(input));
        }

        [TestMethod]
        public void InvalidCastException_ExtraiRespostaTemNovoRegistroTest_Fail()
        {
            string input = "[{}]";
            Assert.ThrowsException<InvalidCastException>(() => OpcApiService.ConverteNovoRegistro(input));
        }

        [TestMethod]
        public void ExtractDataSouth_Sucess()
        {
            DateTime currentTime = DateTime.Now;
            string input = "[{\"Value\":1,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_ORDEM_AMOS_BERCO_TX\",\"FloatValue\":1.0,\"DoubleValue\":1.0,\"Int32Value\":1,\"StringValue\":\"1\"},{\"Value\":[21318,18242,8224,8224,8224,8224,8224,8224,0,0,0,0,0,0,0,0],\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PRODUCT_CODE_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"System.Int16[]\"},{\"Value\":[12848,12851,11569,12854,13104,8224,8224,8224,0,0,0,0,0,0,0,0],\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_BORDING_CODE_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"System.Int16[]\"},{\"Value\":293502,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_ESTIMATED_WEIGHT_TX\",\"FloatValue\":293502.0,\"DoubleValue\":293502.0,\"Int32Value\":293502,\"StringValue\":\"293502\"},{\"Value\":1,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_NUM_1_TX\",\"FloatValue\":1.0,\"DoubleValue\":1.0,\"Int32Value\":1,\"StringValue\":\"1\"},{\"Value\":542,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_TOT_1_PRI_TX\",\"FloatValue\":542.0,\"DoubleValue\":542.0,\"Int32Value\":542,\"StringValue\":\"542\"},{\"Value\":617,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_TOT_1_SEC_TX\",\"FloatValue\":617.0,\"DoubleValue\":617.0,\"Int32Value\":617,\"StringValue\":\"617\"},{\"Value\":4,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_NUM_2_TX\",\"FloatValue\":4.0,\"DoubleValue\":4.0,\"Int32Value\":4,\"StringValue\":\"4\"},{\"Value\":521,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_TOT_2_SEC_TX\",\"FloatValue\":521.0,\"DoubleValue\":521.0,\"Int32Value\":521,\"StringValue\":\"521\"},{\"Value\":541,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_TOT_2_PRI_TX\",\"FloatValue\":541.0,\"DoubleValue\":541.0,\"Int32Value\":541,\"StringValue\":\"541\"},{\"Value\":0,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_NUM_3_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":0,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_TOT_3_SEC_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":0,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_TOT_3_PRI_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":2,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PARCIAL_TX\",\"FloatValue\":2.0,\"DoubleValue\":2.0,\"Int32Value\":2,\"StringValue\":\"2\"},{\"Value\":3,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_SUBPARCIAL_TX\",\"FloatValue\":3.0,\"DoubleValue\":3.0,\"Int32Value\":3,\"StringValue\":\"3\"},{\"Value\":3,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!DB_EXPEDITA_TX\",\"FloatValue\":3.0,\"DoubleValue\":3.0,\"Int32Value\":3,\"StringValue\":\"3\"},{\"Value\":79,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!DB_NUMERO_INCREMENTO_TX\",\"FloatValue\":79.0,\"DoubleValue\":79.0,\"Int32Value\":79,\"StringValue\":\"79\"},{\"Value\":1129,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!DB_PESO_CORTE_PROG_TX\",\"FloatValue\":1129.0,\"DoubleValue\":1129.0,\"Int32Value\":1129,\"StringValue\":\"1129\"}]";
            ColetaFuseData output = OpcApiService.ExtractDataFromPier(input, currentTime);
            var expectedOutput = new ColetaFuseData()
            {
                BOARDING_CODE = "2023-12630",
                BOARDING_LINE = 1,
                CLIENT_CODE = null,
                ESTIMATED_WEIGHT = 293502,
                INCREMENT_DATETIME = currentTime,
                INCREMENT_NUMBER = 79,
                ORDER_NUMBER = null,
                PARTIAL_SAMPLE = "2",
                PIER_CODE = "1S",
                PORAO1_ID = 1,
                PORAO1_PESO1 = 542,
                PORAO1_PESO2 = 617,
                PORAO2_ID = 4,
                PORAO2_PESO1 = 541,
                PORAO2_PESO2 = 521,
                PORAO3_ID = null,
                PORAO3_PESO1 = null,
                PORAO3_PESO2 = null,
                PRODUCT_CODE = "SFGB",
                STACKER_NAME = "CNG2",
                STATUS_TYPE = "Pending",
                SUBPARTIAL_SAMPLE = "C",
                SUBSUBPARTIAL_SAMPLE = "3",
                WEIGHTATCUT = 1129,
                ERRO_LEITURA = 1
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedOutput), JsonConvert.SerializeObject(output));
        }


        [TestMethod]
        public void ExtractDataNorth_Sucess()
        {
            DateTime currentTime = DateTime.Now;
            string input = "[{\"Value\":2,\"TimeStamp\":\"2023-05-03T15:25:26.736-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_ORDEM_AMOS_BERCO_TX\",\"FloatValue\":2.0,\"DoubleValue\":2.0,\"Int32Value\":2,\"StringValue\":\"2\"},{\"Value\":[21318,20295,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"TimeStamp\":\"2023-04-25T11:50:29.615-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PRODUCT_CODE_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"System.Int16[]\"},{\"Value\":[12848,12851,11569,12338,12800,0,0,0,0,0,0,0,0,0,0,0],\"TimeStamp\":\"2023-04-25T11:50:29.615-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_BORDING_CODE_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"System.Int16[]\"},{\"Value\":111411,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_ESTIMATED_WEIGHT_TX\",\"FloatValue\":111411.0,\"DoubleValue\":111411.0,\"Int32Value\":111411,\"StringValue\":\"111411\"},{\"Value\":6,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_NUM_1_TX\",\"FloatValue\":6.0,\"DoubleValue\":6.0,\"Int32Value\":6,\"StringValue\":\"6\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_TOT_1_PRI_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_TOT_1_SEC_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":2,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_NUM_2_TX\",\"FloatValue\":2.0,\"DoubleValue\":2.0,\"Int32Value\":2,\"StringValue\":\"2\"},{\"Value\":568,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_TOT_2_PRI_TX\",\"FloatValue\":568.0,\"DoubleValue\":568.0,\"Int32Value\":568,\"StringValue\":\"568\"},{\"Value\":568,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_TOT_2_SEC_TX\",\"FloatValue\":568.0,\"DoubleValue\":568.0,\"Int32Value\":568,\"StringValue\":\"568\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_NUM_3_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_TOT_3_PRI_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PORAO_TOT_3_SEC_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":3,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_PARCIAL_TX\",\"FloatValue\":3.0,\"DoubleValue\":3.0,\"Int32Value\":3,\"StringValue\":\"3\"},{\"Value\":1,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!BD_SUBPARCIAL_TX\",\"FloatValue\":1.0,\"DoubleValue\":1.0,\"Int32Value\":1,\"StringValue\":\"1\"},{\"Value\":3,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!DB_EXPEDITA_TX\",\"FloatValue\":3.0,\"DoubleValue\":3.0,\"Int32Value\":3,\"StringValue\":\"3\"},{\"Value\":1423,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!DB_NUMERO_INCREMENTO_TX\",\"FloatValue\":0.0,\"DoubleValue\":0.0,\"Int32Value\":0,\"StringValue\":\"0\"}]";
            ColetaFuseData output = OpcApiService.ExtractDataFromPier(input, currentTime);
            var expectedOutput = new ColetaFuseData()
            {
                BOARDING_CODE = "2023-1022",
                BOARDING_LINE = 1,
                CLIENT_CODE = null,
                ESTIMATED_WEIGHT = 111411,
                INCREMENT_DATETIME = currentTime,
                INCREMENT_NUMBER = 1423,
                ORDER_NUMBER = null,
                PARTIAL_SAMPLE = "3",
                PIER_CODE = "1N",
                PORAO1_ID = 6,
                PORAO1_PESO1 = 0,
                PORAO1_PESO2 = 0,
                PORAO2_ID = 2,
                PORAO2_PESO1 = 568,
                PORAO2_PESO2 = 568,
                PORAO3_ID = null,
                PORAO3_PESO1 = null,
                PORAO3_PESO2 = null,
                PRODUCT_CODE = "SFOG",
                STACKER_NAME = "CNG2",
                STATUS_TYPE = "Pending",
                ERRO_LEITURA = 1,
                SUBPARTIAL_SAMPLE = "A",
                SUBSUBPARTIAL_SAMPLE = "3",
                WEIGHTATCUT = null
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedOutput), JsonConvert.SerializeObject(output));
        }

        [TestMethod]
        public void BoardingCode_TransformDouble8BitToString_Sucess()
        {
            var input = new List<int>() { 12848, 12851, 11569, 12338, 12800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            string expectedOutput = "2023-1022";
            Assert.AreEqual(expectedOutput, OpcApiService.TransformDouble8BitToString(input));
        }

        [TestMethod]
        public void ProductCode_TransformDouble8BitToString_Sucess()
        {
            var input = new List<int>() { 21318, 20295, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            string expectedOutput = "SFOG";
            Assert.AreEqual(expectedOutput, OpcApiService.TransformDouble8BitToString(input));
        }
    }
}
