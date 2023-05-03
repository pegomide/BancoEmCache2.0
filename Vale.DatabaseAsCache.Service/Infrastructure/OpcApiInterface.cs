using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        /// <param name="hostname"></param>
        /// <param name="servername"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string PostTemNovoRegistro()
        {
            string responseBody = null;
            try
            {
                var requestBody = new OpcApiRequestBody()
                {
                    Hostname = _hostname,
                    Servername = _servername,
                    Items = new List<string>() { Tag.NewIncrement }
                };
                HttpContent content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = client.PostAsync("", content).Result)
                {
                    _log.Debug($"[PostTemNovoRegistro] RequestItem: {string.Join(",", requestBody.Items)}, StatusCode: {response.StatusCode}");
                    response.EnsureSuccessStatusCode();
                    responseBody = response.Content.ReadAsStringAsync().Result;
                    _log.Debug($"[PostTemNovoRegistro] Response: {responseBody}");
                }
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
        /// Verifica de qual pier é a mensagem
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="servername"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string PostVerificaPier()
        {
            string responseBody = null;
            try
            {
                var requestBody = new OpcApiRequestBody()
                {
                    Hostname = _hostname,
                    Servername = _servername,
                    Items = new List<string>() { Tag.PierCode }
                };
                HttpContent content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = client.PostAsync("", content).Result)
                {
                    _log.Debug($"[PostVerificaPier] RequestItem: {string.Join(",", requestBody.Items)}, StatusCode: {response.StatusCode}");
                    response.EnsureSuccessStatusCode();
                    responseBody = response.Content.ReadAsStringAsync().Result;
                    _log.Debug($"[PostVerificaPier] Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                _log.Error($"Erro na requisição HTTP: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
            }
            catch (Exception ex)
            {
                _log.Error($"Erro genérico ao checar pier: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
            }

            return responseBody;
        }

        /// <summary>
        /// Get data of South pier.
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="servername"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string PostDataSouth()
        {
            string responseBody = null;
            try
            {
                var requestBody = new OpcApiRequestBody()
                {
                    Hostname = _hostname,
                    Servername = _servername,
                    Items = new List<string>() {
                        Tag.ProductCode,
                        Tag.BoardingCodeSouth,
                        Tag.EstimatedWeightSouth,
                        Tag.PoraoID1,
                        Tag.Porao1WeigthFirstScale,
                        Tag.Porao1WeigthSecondScale,
                        Tag.PoraoID2,
                        Tag.Porao2WeigthSecondScale,
                        Tag.Porao2WeigthFirstScale,
                        Tag.PoraoID3,
                        Tag.Porao3WeigthSecondScale,
                        Tag.Porao3WeigthFirstScale,
                        Tag.PartialSampleSouth,
                        Tag.SubPartialSampleSouth,
                        Tag.SubSubPartialSampleSouth,
                        Tag.IncrementNumberSouth,
                        Tag.WeightAtCutSouth,
                        Tag.OrderName
                    }
                };
                HttpContent content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = client.PostAsync("", content).Result)
                {
                    _log.Debug($"[PostDataSouth] RequestItem: {string.Join(",", requestBody.Items)}, StatusCode: {response.StatusCode}");
                    response.EnsureSuccessStatusCode();
                    responseBody = response.Content.ReadAsStringAsync().Result;
                    _log.Debug($"[PostDataSouth] Response: {responseBody}");
                }
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

        /// <summary>
        /// Get data of North pier.
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="servername"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string PostDataNorth()
        {
            string responseBody = null;
            try
            {
                var requestBody = new OpcApiRequestBody()
                {
                    Hostname = _hostname,
                    Servername = _servername,
                    Items = new List<string>() {
                        Tag.ProductCode,
                        Tag.BoardingCodeNorth,
                        Tag.EstimatedWeightNorth,
                        Tag.PoraoID1,
                        Tag.Porao1WeigthFirstScale,
                        Tag.Porao1WeigthSecondScale,
                        Tag.PoraoID2,
                        Tag.Porao2WeigthSecondScale,
                        Tag.Porao2WeigthFirstScale,
                        Tag.PoraoID3,
                        Tag.Porao3WeigthSecondScale,
                        Tag.Porao3WeigthFirstScale,
                        Tag.PartialSampleNorth,
                        Tag.SubPartialSampleNorth,
                        Tag.SubSubPartialSampleNorth,
                        Tag.IncrementNumberNorth,
                        Tag.WeightAtCutNorth,
                        Tag.OrderName
                    }
                };
                HttpContent content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = client.PostAsync("", content).Result)
                {
                    _log.Debug($"[PostDataNorth] RequestItem: {string.Join(",",requestBody.Items)}, StatusCode: {response.StatusCode}");
                    response.EnsureSuccessStatusCode();
                    responseBody = response.Content.ReadAsStringAsync().Result;
                    _log.Debug($"[PostDataNorth] Response: {responseBody}");
                }
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
    }
}
