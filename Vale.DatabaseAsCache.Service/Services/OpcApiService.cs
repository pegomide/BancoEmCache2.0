using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vale.DatabaseAsCache.ApiService.Models;
using Vale.DatabaseAsCache.Data.TableModels;
using Vale.GetFuseData.ApiService.Models;

namespace Vale.GetFuseData.ApiService.Services
{
    public static class OpcApiService
    {
        private static readonly ILog _log = LogManager.GetLogger("log");

        /// <summary>
        /// Checa body da requisição PostVerificaNovoRegistro.
        /// </summary>
        /// <param name="rawResponseBody"></param>
        /// <returns>Conteúdo da resposta avaliada.</returns>
        public static bool ConverteNovoRegistro(string rawResponseBody)
        {
            var responseBodyList = JsonConvert.DeserializeObject<List<OpcApiResponseBody>>(rawResponseBody);
            if (responseBodyList[0].Value == null)
            {
                throw new InvalidCastException();
            }
            return Convert.ToBoolean(responseBodyList[0].Value);
        }

        /// <summary>
        /// Transform an array of 16bit (8 bit + 8 bit) of ASCII items into text.
        /// </summary>
        /// <param name="input">Array of values related with ASCII data, grouped in pairs of 8 bit of hexadecimal data.</param>
        /// <returns>String data as text.</returns>
        public static string TransformDouble8BitToString(List<int> input)
        {
            StringBuilder output = new StringBuilder();
            foreach (int element16Bit in input)
            {
                int highPair = (element16Bit >> 8) & 0xff;
                if (highPair != 0)
                {
                    output.Append(char.ConvertFromUtf32(highPair));
                }
                int lowPair = element16Bit & 0xff;
                if (lowPair != 0)
                {
                    output.Append(char.ConvertFromUtf32(lowPair));
                }
            }
            return output.ToString();
        }

        /// <summary>
        /// Extract data of pier.
        /// </summary>
        /// <param name="rawResponseBody"></param>
        /// <returns></returns>
        public static ColetaFuseData ExtractDataFromPier(string rawResponseBody, DateTime triggerTime)
        {
            var responseBodyList = JsonConvert.DeserializeObject<List<OpcApiResponseBody>>(rawResponseBody);

            if (responseBodyList.Count.Equals(0))
            {
                return null;
            }

            ColetaFuseData fuseData = new ColetaFuseData
            {
                INCREMENT_DATETIME = triggerTime,
                STATUS_TYPE = SendingStatus.Pending.ToString(),
            };
            foreach (OpcApiResponseBody body in responseBodyList)
            {
                ProcessBodyFromResponse(responseBodyList, fuseData, body);
            }

            ValidateRequiredFields(fuseData);
            return fuseData;
        }

        /// <summary>
        /// Extrai informações do corpo da resposta.
        /// </summary>
        /// <param name="responseBodyList"></param>
        /// <param name="fuseData"></param>
        /// <param name="body"></param>
        private static void ProcessBodyFromResponse(List<OpcApiResponseBody> responseBodyList, ColetaFuseData fuseData, OpcApiResponseBody body)
        {
            if (body.Name.Equals(Tag.PierCode))
            {
                var pier = (PierCode)Convert.ToInt32(body.Value);
                fuseData.PIER_CODE = GetPierCode(pier);
            }
            else if (body.Name.Equals(Tag.BoardingCode))
            {
                List<int> inputListAux = ((IEnumerable<object>)body.Value).Select(x => Convert.ToInt32(x)).ToList();
                fuseData.BOARDING_CODE = TransformDouble8BitToString(inputListAux).TrimEnd();
            }
            else if (body.Name.Equals(Tag.IncrementNumber))
            {
                fuseData.INCREMENT_NUMBER = Convert.ToInt32(body.Value);
            }
            else if (body.Name.Equals(Tag.ProductCode))
            {
                List<int> inputListAux = ((IEnumerable<object>)body.Value).Select(x => Convert.ToInt32(x)).ToList();
                fuseData.PRODUCT_CODE = TransformDouble8BitToString(inputListAux).TrimEnd();
            }
            else if (body.Name.Equals(Tag.EstimatedWeight))
            {
                fuseData.ESTIMATED_WEIGHT = Convert.ToDecimal(body.Value);
            }
            else if (body.Name.Equals(Tag.PoraoID1))
            {
                if (body.Value != null)
                {
                    int id = Convert.ToInt32(body.Value);
                    if (id > 0 && id <= 30)
                    {
                        fuseData.PORAO1_ID = id;
                        fuseData.PORAO1_PESO1 = Convert.ToDecimal(responseBodyList.Find(item => item.Name.Equals(Tag.Porao1WeigthFirstScale)).Value);
                        fuseData.PORAO1_PESO2 = Convert.ToDecimal(responseBodyList.Find(item => item.Name.Equals(Tag.Porao1WeigthSecondScale)).Value);
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
                        fuseData.PORAO2_PESO1 = Convert.ToDecimal(responseBodyList.Find(item => item.Name.Equals(Tag.Porao2WeigthFirstScale)).Value);
                        fuseData.PORAO2_PESO2 = Convert.ToDecimal(responseBodyList.Find(item => item.Name.Equals(Tag.Porao2WeigthSecondScale)).Value);
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
                        fuseData.PORAO3_PESO1 = Convert.ToDecimal(responseBodyList.Find(item => item.Name.Equals(Tag.Porao3WeigthFirstScale)).Value);
                        fuseData.PORAO3_PESO2 = Convert.ToDecimal(responseBodyList.Find(item => item.Name.Equals(Tag.Porao3WeigthSecondScale)).Value);
                    }
                }
            }
            else if (body.Name.Equals(Tag.OrderName))
            {
                fuseData.ORDER_NUMBER = Convert.ToInt32(body.Value);
            }
            else if (body.Name.Equals(Tag.PartialSample))
            {
                fuseData.PARTIAL_SAMPLE = Convert.ToString(body.Value);
            }
            else if (body.Name.Equals(Tag.SubPartialSample))
            {
                fuseData.SUBPARTIAL_SAMPLE = ((SubPartial)Convert.ToInt32(body.Value)).ToString();
            }
            else if (body.Name.Equals(Tag.SubSubPartialSample))
            {
                fuseData.SUBSUBPARTIAL_SAMPLE = Convert.ToString(body.Value);
            }
            else if (body.Name.Equals(Tag.WeightAtCut))
            {
                fuseData.WEIGHTATCUT = Convert.ToDecimal(body.Value);
            }
            else
            {
                _log.ErrorFormat("Tag inválida ao tratar dados do pier: {0}", body.Name);
            }
        }

        /// <summary>
        /// Obtem código do pier do enum.
        /// </summary>
        /// <param name="pier"></param>
        /// <returns></returns>
        private static string GetPierCode(PierCode pier)
        {
            if (pier.Equals(PierCode.South))
            {
                return "1S";
            }
            else if (pier.Equals(PierCode.North))
            {
                return "1N";
            }
            return "NONE";
        }

        /// <summary>
        /// Valida se todos os campos obrigatórios foram lidos corretamente do OPC.
        /// </summary>
        /// <param name="fuseData"></param>
        private static void ValidateRequiredFields(ColetaFuseData fuseData)
        {
            if (string.IsNullOrEmpty(fuseData.PIER_CODE)
                || string.IsNullOrEmpty(fuseData.BOARDING_CODE)
                || fuseData.INCREMENT_NUMBER == 0
                || string.IsNullOrEmpty(fuseData.PRODUCT_CODE)
                || fuseData.ESTIMATED_WEIGHT.GetValueOrDefault() == 0
                || fuseData.ORDER_NUMBER.GetValueOrDefault() == 0
                || fuseData.BOARDING_LINE.GetValueOrDefault() == 0
                || string.IsNullOrEmpty(fuseData.PARTIAL_SAMPLE)
                || string.IsNullOrEmpty(fuseData.SUBPARTIAL_SAMPLE)
                || string.IsNullOrEmpty(fuseData.SUBSUBPARTIAL_SAMPLE)
                || fuseData.WEIGHTATCUT.GetValueOrDefault() == 0)
            {
                fuseData.ERRO_LEITURA = 1;
                _log.ErrorFormat("Campo obrigatório não foi lido corretamente do OPC: {0}",
                    (string.IsNullOrEmpty(fuseData.PIER_CODE) ? $"PIER_CODE " : string.Empty) +
                    (string.IsNullOrEmpty(fuseData.BOARDING_CODE) ? $"BOARDING_CODE " : string.Empty) +
                    (fuseData.INCREMENT_NUMBER == 0 ? $"INCREMENT_NUMBER " : string.Empty) +
                    (string.IsNullOrEmpty(fuseData.PRODUCT_CODE) ? $"PRODUCT_CODE " : string.Empty) +
                    (fuseData.ESTIMATED_WEIGHT.GetValueOrDefault() == 0 ? $"ESTIMATED_WEIGHT " : string.Empty) +
                    (fuseData.ORDER_NUMBER.GetValueOrDefault() == 0 ? $"ORDER_NUMBER " : string.Empty) +
                    (fuseData.BOARDING_LINE.GetValueOrDefault() == 0 ? $"BOARDING_LINE " : string.Empty) +
                    (string.IsNullOrEmpty(fuseData.PARTIAL_SAMPLE) ? $"PARTIAL_SAMPLE " : string.Empty) +
                    (string.IsNullOrEmpty(fuseData.SUBPARTIAL_SAMPLE) ? $"SUBPARTIAL_SAMPLE " : string.Empty) +
                    (string.IsNullOrEmpty(fuseData.SUBSUBPARTIAL_SAMPLE) ? $"SUBSUBPARTIAL_SAMPLE " : string.Empty) +
                    (fuseData.WEIGHTATCUT.GetValueOrDefault() == 0 ? $"WEIGHTATCUT " : string.Empty));
            }
        }
    }
}
