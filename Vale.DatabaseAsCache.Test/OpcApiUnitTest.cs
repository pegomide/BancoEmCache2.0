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
            string input = "[{\"Value\":false,\"TimeStamp\":\"2023-04-18T15:35:28.319-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_NEW_INCR\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"False\"}]";
            bool retorno = OpcApiService.TemNovoRegistro(input);
            Assert.IsFalse(retorno);
        }

        [TestMethod]
        public void IsTrue_ExtraiRespostaTemNovoRegistroTest_Sucess()
        {
            string input = "[{\"Value\":true,\"TimeStamp\":\"2023-04-18T15:35:28.319-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_NEW_INCR\",\"FloatValue\":1,\"DoubleValue\":1,\"Int32Value\":1,\"StringValue\":\"True\"}]";
            bool retorno = OpcApiService.TemNovoRegistro(input);
            Assert.IsTrue(retorno);
        }

        [TestMethod]
        public void JsonSerializationException_ExtraiRespostaTemNovoRegistroTest_Fail()
        {
            string input = "{\"Value\":true,\"TimeStamp\":\"2023-04-18T15:35:28.319-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_NEW_INCR\",\"FloatValue\":1,\"DoubleValue\":1,\"Int32Value\":1,\"StringValue\":\"True\"}";
            Assert.ThrowsException<JsonSerializationException>(() => OpcApiService.TemNovoRegistro(input));
        }

        [TestMethod]
        public void InvalidCastException_ExtraiRespostaTemNovoRegistroTest_Fail()
        {
            string input = "[{}]";
            Assert.ThrowsException<InvalidCastException>(() => OpcApiService.TemNovoRegistro(input));
        }

        [TestMethod]
        public void ExtractDataSouth_Sucess()
        {
            DateTime currentTime = DateTime.Now;
            string input = "[{\"Value\":[21318,20295,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"TimeStamp\":\"2023-04-25T11:50:29.615-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_C20_MW29706_PRODUTO\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"System.Int16[]\"},{\"Value\":[12848,12851,11569,12338,12800,0,0,0,0,0,0,0,0,0,0,0],\"TimeStamp\":\"2023-04-25T11:50:29.615-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_TIG_CNG02_GRL_SHIP_01_D\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"System.Int16[]\"},{\"Value\":288300,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_CN01_BERCS_TOT_01_D\",\"FloatValue\":288300,\"DoubleValue\":288300,\"Int32Value\":288300,\"StringValue\":\"288300\"},{\"Value\":6,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_NUM_1_D\",\"FloatValue\":6,\"DoubleValue\":6,\"Int32Value\":6,\"StringValue\":\"6\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_TOT_1_D\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_TOT08_1_D\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":2,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_NUM_2_D\",\"FloatValue\":2,\"DoubleValue\":2,\"Int32Value\":2,\"StringValue\":\"2\"},{\"Value\":568,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_TOT_2_D\",\"FloatValue\":568,\"DoubleValue\":568,\"Int32Value\":568,\"StringValue\":\"568\"},{\"Value\":568,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_TOT08_2_D\",\"FloatValue\":568,\"DoubleValue\":568,\"Int32Value\":568,\"StringValue\":\"568\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_NUM_3_D\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_TOT_3_D\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_TOT08_3_D\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":6,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_SUL_PARC_D\",\"FloatValue\":6,\"DoubleValue\":6,\"Int32Value\":6,\"StringValue\":\"6\"},{\"Value\":3,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AMOS_SUL_SP_VAL_QUAR\",\"FloatValue\":3,\"DoubleValue\":3,\"Int32Value\":3,\"StringValue\":\"3\"},{\"Value\":3,\"TimeStamp\":\"2023-04-18T15:42:59.671-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_SUL_QUA_SPED_VAL\",\"FloatValue\":3,\"DoubleValue\":3,\"Int32Value\":3,\"StringValue\":\"3\"}]";
            var output = OpcApiService.ExtractDataSouth(input, currentTime);
            var expectedOutput = new ColetaFuseData()
            {
                BOARDING_CODE = "2023-1022",
                BOARDING_LINE = null,
                CLIENT_CODE = null,
                ESTIMATED_WEIGHT = 288300,
                INCREMENT_DATETIME = currentTime,
                INCREMENT_NUMBER = 0,
                ORDER_NUMBER = null,
                PARTIAL_SAMPLE = "6",
                PIER_CODE = "South",
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
                STACKER_NAME = "CN02",
                STATUS_TYPE = "Pending",
                SUBPARTIAL_SAMPLE = "C",
                SUBSUBPARTIAL_SAMPLE = "3",
                WEIGHTATCUT = null
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedOutput), JsonConvert.SerializeObject(output));
        }

        [TestMethod]
        public void ExtractDataNorth_Sucess()
        {
            DateTime currentTime = DateTime.Now;
            string input = "[{\"Value\":[21318,20295,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"TimeStamp\":\"2023-04-25T11:50:29.615-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_C20_MW29706_PRODUTO\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"System.Int16[]\"},{\"Value\":[12848,12851,11569,12338,12800,0,0,0,0,0,0,0,0,0,0,0],\"TimeStamp\":\"2023-04-25T11:50:29.615-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_TIG_CNG01_GRL_SHIP_01_D\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"System.Int16[]\"},{\"Value\":111411,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_CN01_BERCN_TOT_01_D\",\"FloatValue\":111411,\"DoubleValue\":111411,\"Int32Value\":111411,\"StringValue\":\"111411\"},{\"Value\":6,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_NUM_1_D\",\"FloatValue\":6,\"DoubleValue\":6,\"Int32Value\":6,\"StringValue\":\"6\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_TOT_1_D\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_TOT08_1_D\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":2,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_NUM_2_D\",\"FloatValue\":2,\"DoubleValue\":2,\"Int32Value\":2,\"StringValue\":\"2\"},{\"Value\":568,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_TOT_2_D\",\"FloatValue\":568,\"DoubleValue\":568,\"Int32Value\":568,\"StringValue\":\"568\"},{\"Value\":568,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_TOT08_2_D\",\"FloatValue\":568,\"DoubleValue\":568,\"Int32Value\":568,\"StringValue\":\"568\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_NUM_3_D\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_TOT_3_D\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":0,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_PORAO_TOT08_3_D\",\"FloatValue\":0,\"DoubleValue\":0,\"Int32Value\":0,\"StringValue\":\"0\"},{\"Value\":3,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_NORTE_PARC_D\",\"FloatValue\":3,\"DoubleValue\":3,\"Int32Value\":3,\"StringValue\":\"3\"},{\"Value\":1,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_NORTE_SP_VAL_QUAR\",\"FloatValue\":1,\"DoubleValue\":1,\"Int32Value\":1,\"StringValue\":\"1\"},{\"Value\":3,\"TimeStamp\":\"2023-04-18T15:53:41.239-03:00\",\"Quality\":\"good\",\"Name\":\"EMB!PIMS_AM_NORTE_QUA_SPED_VAL\",\"FloatValue\":3,\"DoubleValue\":3,\"Int32Value\":3,\"StringValue\":\"3\"}]";
            var output = OpcApiService.ExtractDataNorth(input, currentTime);
            var expectedOutput = new ColetaFuseData()
            {
                BOARDING_CODE = "2023-1022",
                BOARDING_LINE = null,
                CLIENT_CODE = null,
                ESTIMATED_WEIGHT = 111411,
                INCREMENT_DATETIME = currentTime,
                INCREMENT_NUMBER = 0,
                ORDER_NUMBER = null,
                PARTIAL_SAMPLE = "3",
                PIER_CODE = "North",
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
                STACKER_NAME = "CN02",
                STATUS_TYPE = "Pending",
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
