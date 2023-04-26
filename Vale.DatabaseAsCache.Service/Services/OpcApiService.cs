using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Vale.DatabaseAsCache.ApiService.Models;
using Vale.DatabaseAsCache.Data.TableModels;
using Vale.GetFuseData.ApiService.Models;

namespace Vale.GetFuseData.ApiService.Services
{
    public static class OpcApiService
    {
        private static readonly ILog _log = LogManager.GetLogger("log");

        /// <summary>
        /// Checa body da requisição PostTemNovoRegistro.
        /// </summary>
        /// <param name="rawResponseBody"></param>
        /// <returns>Conteúdo da resposta avaliada.</returns>
        public static bool TemNovoRegistro(string rawResponseBody)
        {
            var responseBodyList = JsonConvert.DeserializeObject<List<OpcApiResponseBody>>(rawResponseBody);
            if (responseBodyList[0].Value == null)
            {
                throw new InvalidCastException();
            }
            return Convert.ToBoolean(responseBodyList[0].Value);
        }

        /// <summary>
        /// Checa de qual pier é a mensagem.
        /// </summary>
        /// <param name="rawResponseBody"></param>
        /// <returns>Conteúdo da resposta avaliada.</returns>
        public static PierEnum ExtraiPier(string rawResponseBody)
        {
            var responseBodyList = JsonConvert.DeserializeObject<List<OpcApiResponseBody>>(rawResponseBody);
            if (responseBodyList[0].Value == null)
            {
                throw new InvalidCastException();
            }
            return (PierEnum)Convert.ToInt32(responseBodyList[0].Value);
        }

        /// <summary>
        /// Extract data of south pier.
        /// </summary>
        /// <param name="rawResponseBody"></param>
        /// <returns></returns>
        public static ColetaFuseData ExtractDataSouth(string rawResponseBody, DateTime triggerTime)
        {
            var responseBodyList = JsonConvert.DeserializeObject<List<OpcApiResponseBody>>(rawResponseBody);

            ColetaFuseData fuseData = new ColetaFuseData
            {
                PIER_CODE = "1S",
                INCREMENT_DATETIME = triggerTime,
                STATUS_TYPE = StatusEnum.Pending.ToString(),
            };
            List<int> inputListAux;
            foreach (OpcApiResponseBody body in responseBodyList)
            {
                if (body.Name.Equals(Tag.Porao1WeigthFirstScale)
                    || body.Name.Equals(Tag.Porao1WeigthSecondScale)
                    || body.Name.Equals(Tag.Porao2WeigthFirstScale)
                    || body.Name.Equals(Tag.Porao2WeigthSecondScale)
                    || body.Name.Equals(Tag.Porao3WeigthFirstScale)
                    || body.Name.Equals(Tag.Porao3WeigthSecondScale))
                {
                    continue;
                }
                else if (body.Name.Equals(Tag.BoardingCodeSouth))
                {
                    inputListAux = ((IEnumerable<object>)body.Value).Select(x => Convert.ToInt32(x)).ToList();
                    fuseData.BOARDING_CODE = TransformDouble8BitToString(inputListAux);
                }
                else if (body.Name.Equals(Tag.IncrementNumberSouth))
                {
                    fuseData.INCREMENT_NUMBER = Convert.ToInt32(body.Value);
                }
                else if (body.Name.Equals(Tag.ProductCode))
                {
                    inputListAux = ((IEnumerable<object>)body.Value).Select(x => Convert.ToInt32(x)).ToList();
                    fuseData.PRODUCT_CODE = TransformDouble8BitToString(inputListAux);
                }
                else if (body.Name.Equals(Tag.EstimatedWeightSouth))
                {
                    fuseData.ESTIMATED_WEIGHT = Convert.ToDecimal(body.Value);
                }
                else if (body.Name.Equals(Tag.PoraoID1))
                {
                    if (body.Value != null)
                    {
                        var id = Convert.ToInt32(body.Value);
                        if (id > 0 && id <= 30)
                        {
                            fuseData.PORAO1_ID = id;
                            var peso1 = responseBodyList.Find(item => item.Name.Equals(Tag.Porao1WeigthFirstScale));
                            fuseData.PORAO1_PESO1 = Convert.ToDecimal(peso1.Value);
                            var peso2 = responseBodyList.Find(item => item.Name.Equals(Tag.Porao1WeigthSecondScale));
                            fuseData.PORAO1_PESO2 = Convert.ToDecimal(peso2.Value);
                        }
                    }
                }
                else if (body.Name.Equals(Tag.PoraoID2))
                {
                    if (body.Value != null)
                    {
                        var id = Convert.ToInt32(body.Value);
                        if (id > 0 && id <= 30)
                        {
                            fuseData.PORAO2_ID = id;
                            var peso1 = responseBodyList.Find(item => item.Name.Equals(Tag.Porao2WeigthFirstScale));
                            fuseData.PORAO2_PESO1 = Convert.ToDecimal(peso1.Value);
                            var peso2 = responseBodyList.Find(item => item.Name.Equals(Tag.Porao2WeigthSecondScale));
                            fuseData.PORAO2_PESO2 = Convert.ToDecimal(peso2.Value);
                        }
                    }
                }
                else if (body.Name.Equals(Tag.PoraoID3))
                {
                    if (body.Value != null)
                    {
                        var id = Convert.ToInt32(body.Value);
                        if (id > 0 && id <= 30)
                        {
                            fuseData.PORAO3_ID = id;
                            var peso1 = responseBodyList.Find(item => item.Name.Equals(Tag.Porao3WeigthFirstScale));
                            fuseData.PORAO3_PESO1 = Convert.ToDecimal(peso1.Value);
                            var peso2 = responseBodyList.Find(item => item.Name.Equals(Tag.Porao3WeigthSecondScale));
                            fuseData.PORAO3_PESO2 = Convert.ToDecimal(peso2.Value);
                        }
                    }
                }
                else if (body.Name.Equals(Tag.OrderName))
                {
                    fuseData.ORDER_NUMBER = Convert.ToInt32(body.Value);
                }
                else if (body.Name.Equals(Tag.PartialSampleSouth))
                {
                    fuseData.PARTIAL_SAMPLE = Convert.ToString(body.Value);
                }
                else if (body.Name.Equals(Tag.SubPartialSampleSouth))
                {
                    fuseData.SUBPARTIAL_SAMPLE = ((SubPartialEnum)Convert.ToInt32(body.Value)).ToString();
                }
                else if (body.Name.Equals(Tag.SubSubPartialSampleSouth))
                {
                    fuseData.SUBSUBPARTIAL_SAMPLE = Convert.ToString(body.Value);
                }
                else if (body.Name.Equals(Tag.WeightAtCutSouth))
                {
                    fuseData.WEIGHTATCUT = Convert.ToDecimal(body.Value);
                }
                else
                {
                    _log.Error("Tag inválida ao tratar dados do sul");
                    break;
                }
            }
            return fuseData;
        }

        /// <summary>
        /// Extract data of North pier.
        /// </summary>
        /// <param name="rawResponseBody"></param>
        /// <returns></returns>
        public static ColetaFuseData ExtractDataNorth(string rawResponseBody, DateTime triggerTime)
        {
            var responseBodyList = JsonConvert.DeserializeObject<List<OpcApiResponseBody>>(rawResponseBody);

            ColetaFuseData fuseData = new ColetaFuseData
            {
                PIER_CODE = "1N",
                INCREMENT_DATETIME = triggerTime,
                STATUS_TYPE = StatusEnum.Pending.ToString(),
            };
            List<int> inputListAux;
            foreach (OpcApiResponseBody body in responseBodyList)
            {
                if (body.Name.Equals(Tag.Porao1WeigthFirstScale)
                    || body.Name.Equals(Tag.Porao1WeigthSecondScale)
                    || body.Name.Equals(Tag.Porao2WeigthFirstScale)
                    || body.Name.Equals(Tag.Porao2WeigthSecondScale)
                    || body.Name.Equals(Tag.Porao3WeigthFirstScale)
                    || body.Name.Equals(Tag.Porao3WeigthSecondScale))
                {
                    continue;
                }
                else if (body.Name.Equals(Tag.BoardingCodeNorth))
                {
                    inputListAux = ((IEnumerable<object>)body.Value).Select(x => Convert.ToInt32(x)).ToList();
                    fuseData.BOARDING_CODE = TransformDouble8BitToString(inputListAux);
                }
                else if (body.Name.Equals(Tag.IncrementNumberNorth))
                {
                    fuseData.INCREMENT_NUMBER = Convert.ToInt32(body.Value);
                }
                else if (body.Name.Equals(Tag.ProductCode))
                {
                    inputListAux = ((IEnumerable<object>)body.Value).Select(x => Convert.ToInt32(x)).ToList();
                    fuseData.PRODUCT_CODE = TransformDouble8BitToString(inputListAux);
                }
                else if (body.Name.Equals(Tag.EstimatedWeightNorth))
                {
                    fuseData.ESTIMATED_WEIGHT = Convert.ToDecimal(body.Value);
                }
                else if (body.Name.Equals(Tag.PoraoID1))
                {
                    if (body.Value != null)
                    {
                        var id = Convert.ToInt32(body.Value);
                        if (id > 0 && id <= 30)
                        {
                            fuseData.PORAO1_ID = id;
                            var peso1 = responseBodyList.Find(item => item.Name.Equals(Tag.Porao1WeigthFirstScale));
                            fuseData.PORAO1_PESO1 = Convert.ToDecimal(peso1.Value);
                            var peso2 = responseBodyList.Find(item => item.Name.Equals(Tag.Porao1WeigthSecondScale));
                            fuseData.PORAO1_PESO2 = Convert.ToDecimal(peso2.Value);
                        }
                    }
                }
                else if (body.Name.Equals(Tag.PoraoID2))
                {
                    if (body.Value != null)
                    {
                        var id = Convert.ToInt32(body.Value);
                        if (id > 0 && id <= 30)
                        {
                            fuseData.PORAO2_ID = id;
                            var peso1 = responseBodyList.Find(item => item.Name.Equals(Tag.Porao2WeigthFirstScale));
                            fuseData.PORAO2_PESO1 = Convert.ToDecimal(peso1.Value);
                            var peso2 = responseBodyList.Find(item => item.Name.Equals(Tag.Porao2WeigthSecondScale));
                            fuseData.PORAO2_PESO2 = Convert.ToDecimal(peso2.Value);
                        }
                    }
                }
                else if (body.Name.Equals(Tag.PoraoID3))
                {
                    if (body.Value != null)
                    {
                        var id = Convert.ToInt32(body.Value);
                        if (id > 0 && id <= 30)
                        {
                            fuseData.PORAO3_ID = id;
                            var peso1 = responseBodyList.Find(item => item.Name.Equals(Tag.Porao3WeigthFirstScale));
                            fuseData.PORAO3_PESO1 = Convert.ToDecimal(peso1.Value);
                            var peso2 = responseBodyList.Find(item => item.Name.Equals(Tag.Porao3WeigthSecondScale));
                            fuseData.PORAO3_PESO2 = Convert.ToDecimal(peso2.Value);
                        }
                    }
                }
                else if (body.Name.Equals(Tag.OrderName))
                {
                    fuseData.ORDER_NUMBER = Convert.ToInt32(body.Value);
                }
                else if (body.Name.Equals(Tag.PartialSampleNorth))
                {
                    fuseData.PARTIAL_SAMPLE = Convert.ToString(body.Value);
                }
                else if (body.Name.Equals(Tag.SubPartialSampleNorth))
                {
                    fuseData.SUBPARTIAL_SAMPLE = ((SubPartialEnum)Convert.ToInt32(body.Value)).ToString();
                }
                else if (body.Name.Equals(Tag.SubSubPartialSampleNorth))
                {
                    fuseData.SUBSUBPARTIAL_SAMPLE = Convert.ToString(body.Value);
                }
                else if (body.Name.Equals(Tag.WeightAtCutNorth))
                {
                    fuseData.WEIGHTATCUT = Convert.ToDecimal(body.Value);
                }
                else
                {
                    _log.Error($"Tag inválida ao tratar dados do norte: {body.Name}");
                }
            }
            return fuseData;
        }

        /// <summary>
        /// Transform an array of 16bit (8 bit + 8 bit) of ASCII items into text.
        /// </summary>
        /// <param name="input">Array of values related with ASCII data, grouped in pairs of 8 bit of hexadecimal data.</param>
        /// <returns>String data as text.</returns>
        public static string TransformDouble8BitToString(List<int> input)
        {
            string output = string.Empty;
            foreach (int element16Bit in input)
            {
                int highPair = (element16Bit >> 8) & 0xff;
                if (highPair != 0)
                {
                    output += char.ConvertFromUtf32(highPair);
                }
                int lowPair = element16Bit & 0xff;
                if (lowPair != 0)
                {
                    output += char.ConvertFromUtf32(lowPair);
                }
            }
            return output;
        }
    }
}
