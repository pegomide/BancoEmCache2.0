using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Vale.DatabaseAsCache.ApiService.Models;

namespace Vale.DatabaseAsCache.Service.Infrastructure
{
    public class OpcApiInterface
    {
        private static readonly ILog _log = LogManager.GetLogger("log");
        private readonly HttpClient client;
        private readonly string _hostname;
        private readonly string _servername;

        public OpcApiInterface(OpcApiOptions opcApiOptions)
        {
            _hostname = opcApiOptions.HostName;
            _servername = opcApiOptions.ServerName;

            HttpClientHandler handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };

            // URL must end with slash: https://www.rfc-editor.org/rfc/rfc3986
            string baseUrl = opcApiOptions.OpcApiUrl;
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl.Append('/');
            }
            client = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseUrl)
            };
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("ContentType", "application/json; charset=utf-8");
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.Timeout = TimeSpan.FromSeconds(60);
        }

        /// <summary>
        /// Verifica se há novo registro disponível
        /// </summary>
        /// <returns>Corpo da resposta em formato string</returns>
        public string PostVerificaNovoRegistro()
        {
            string responseBody = null;
            try
            {
                OpcApiRequestBody requestBody = new OpcApiRequestBody()
                {
                    Hostname = _hostname,
                    Servername = _servername,
                    Items = new List<string>() { Tag.NewIncrement }
                };
                responseBody = PostOpcRequest(requestBody, OpcRequestType.Read);
            }
            catch (HttpRequestException ex)
            {
                _log.Error($"Erro na requisição HTTP: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
            }
            catch (Exception ex)
            {
                _log.Error($"Erro genérico ao checar se há novo registro: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
            }

            return responseBody;
        }

        /// <summary>
        /// Get data of South or North pier.
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="servername"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string PostDataFromPier()
        {
            string responseBody = null;
            try
            {
                OpcApiRequestBody requestBody = new OpcApiRequestBody()
                {
                    Hostname = _hostname,
                    Servername = _servername,
                    Items = new List<string>() {
                        Tag.PierCode,
                        Tag.ProductCode,
                        Tag.BoardingCode,
                        Tag.EstimatedWeight,
                        Tag.PoraoID1,
                        Tag.Porao1WeigthFirstScale,
                        Tag.Porao1WeigthSecondScale,
                        Tag.PoraoID2,
                        Tag.Porao2WeigthSecondScale,
                        Tag.Porao2WeigthFirstScale,
                        Tag.PoraoID3,
                        Tag.Porao3WeigthSecondScale,
                        Tag.Porao3WeigthFirstScale,
                        Tag.PartialSample,
                        Tag.SubPartialSample,
                        Tag.SubSubPartialSample,
                        Tag.IncrementNumber,
                        Tag.WeightAtCut,
                        Tag.OrderName
                    }
                };
                responseBody = PostOpcRequest(requestBody, OpcRequestType.Read);
            }
            catch (HttpRequestException ex)
            {
                _log.Error($"Erro na requisição HTTP: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
            }
            catch (Exception ex)
            {
                _log.Error($"Erro genérico ao extrair dados do pier sul: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
            }

            return responseBody;
        }

        public bool PostSendWatdogSignal(bool lastValueSent)
        {
            try
            {
                OpcApiRequestBody requestBody = new OpcApiRequestBody()
                {
                    Hostname = _hostname,
                    Servername = _servername,
                    Items = new List<string>() { Tag.WatchdogSignal },
                    Values = new List<int>() { Convert.ToInt32(!lastValueSent) }
                };
                string responseBody = PostOpcRequest(requestBody, OpcRequestType.Write);
                _log.Info($"Sinal de watchdog enviado com sucesso. Valor: {requestBody.Values.First()}");
            }
            catch (HttpRequestException ex)
            {
                _log.Error($"Erro na requisição HTTP: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
            }
            catch (Exception ex)
            {
                _log.Error($"Erro genérico ao extrair dados do pier sul: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
            }
            return true;
        }

        private enum OpcRequestType
        {
            Read,
            Write
        }

        /// <summary>
        /// Método comum para ler ou setar dados no servidor OPC
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="callerName"></param>
        /// <returns>Corpo da resposta da requisição. No caso de setar os dados, é retornado uma confirmação.</returns>
        private string PostOpcRequest(OpcApiRequestBody requestBody, OpcRequestType requestType, [CallerMemberName] string callerName = "")
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = client.PostAsync(requestType.ToString().ToLower(CultureInfo.InvariantCulture), content).Result)
            {
                _log.Debug($"[{callerName}] RequestItem: {string.Join(",", requestBody.Items)}, StatusCode: {response.StatusCode}");
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                _log.Debug($"[{callerName}] Response: {responseBody}");
                return responseBody;
            }
        }
    }
}
