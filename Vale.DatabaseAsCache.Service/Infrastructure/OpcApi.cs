using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Vale.GetFuseData.ApiService.Models;

namespace Vale.DatabaseAsCache.Service.Infrastructure
{
    public class OpcApi
    {
        private static readonly ILog _log = LogManager.GetLogger("log");
        private readonly HttpClient client;

        public OpcApi(string certificatePath, string baseUrl)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };
            if (certificatePath != null)
            {
                handler.ClientCertificates.Add(new X509Certificate2(certificatePath));
            }

            // URL must end with slash: https://www.rfc-editor.org/rfc/rfc3986
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl.Append('/');
            }
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(baseUrl);
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
        public bool TemNovoRegistro(string hostname, string servername, string tag)
        {
            try
            {
                var requestBody = new OpcApiRequestBody()
                {
                    Hostname = hostname,
                    Servername = servername,
                    Items = new List<string>() { tag }
                };
                HttpContent content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = client.PostAsync("", content).Result)
                {
                    response.EnsureSuccessStatusCode();
                    return ExtraiRespostaTemNovoRegistro(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (InvalidCastException ex)
            {
                _log.Error($"Impossível converter resposta da API: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
                return false;
            }
            catch (HttpRequestException ex)
            {
                _log.Error($"Erro na requisição HTTP: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
                return false;

            }
            catch (Exception ex)
            {
                _log.Error($"Erro genérico ao checar se há novo registro: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
                return false;
            }
        }

        /// <summary>
        /// Checa body da requisição TemNovoRegistro.
        /// </summary>
        /// <param name="rawResponseBody"></param>
        /// <returns>Conteúdo da resposta avaliada.</returns>
        public bool ExtraiRespostaTemNovoRegistro(string rawResponseBody)
        {
            var responseBody = JsonConvert.DeserializeObject<List<OpcApiResponseBody>>(rawResponseBody);
            if(responseBody[0].Value == null)
            {
                throw new InvalidCastException();
            }
            return Convert.ToBoolean(responseBody[0].Value);
        }
    }
}
