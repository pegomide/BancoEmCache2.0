using log4net;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Vale.GetFuseData.ApiService.Models;

namespace Vale.DatabaseAsCache.Service.Infrastructure
{
    public class FuseApiInterface
    {
        private static readonly ILog _log = LogManager.GetLogger("log");
        private readonly HttpClient client;

        public FuseApiInterface(FuseApiOptions fuseApiOptions)
        {

            HttpClientHandler handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };

            // URL must end with slash: https://www.rfc-editor.org/rfc/rfc3986
            string baseUrl = fuseApiOptions.FuseApiUrl;
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl.Append('/');
            }
            client = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseUrl)
            };
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.Timeout = TimeSpan.FromSeconds(60);
        }

        /// <summary>
        /// Verifica se há novo registro disponível
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool PostSendData(FuseApiRequestBody body)
        {
            try
            {
                HttpContent content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = client.PostAsync("", content).Result)
                {
                    _log.Debug($"PostSendData response: {JsonConvert.SerializeObject(response)}");
                    response.EnsureSuccessStatusCode();
                    return true;
                }
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
    }
}
