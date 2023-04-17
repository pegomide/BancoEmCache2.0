using System.Xml.Serialization;
using Vale.GetFuseData.Service.Models;
using System.Net.Http;
using System;
using log4net;
using System.Xml.Linq;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Vale.GetFuseData.Service.Infrastructure
{
    public class FuseApiIntegration
    {
        private static readonly ILog _log = LogManager.GetLogger("log");
        private readonly HttpClient client;

        public FuseApiIntegration(string certificatePath)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };
            handler.ClientCertificates.Add(new X509Certificate2(certificatePath));

            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Accept", "application/xml");
            client.DefaultRequestHeaders.Add("ContentType", "application/xml; charset=utf-8");
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.Timeout = TimeSpan.FromSeconds(60);
        }

        public EmbarqueDadosQualidadeXML GetEmbarqueDadosQualidade(string url)
        {
            try
            {
                // HTTP request
                _log.Debug($"Enviando requisição EmbarqueDadosQualidade: {url.ToString().Replace(Environment.NewLine, string.Empty)}");
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    response.EnsureSuccessStatusCode();
                    _log.Debug($"Recebeu resposta EmbarqueDadosQualidade: {response.ToString().Replace(Environment.NewLine, string.Empty)}");
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    _log.Debug($"Corpo da resposta EmbarqueDadosQualidade: {responseBody.ToString().Replace(Environment.NewLine, string.Empty)}");

                    // XML handling
                    var serializer = new XmlSerializer(typeof(EmbarqueDadosQualidadeXML));
                    XDocument xdoc = XDocument.Parse(responseBody);
                    xdoc.Descendants().Where(e => string.IsNullOrEmpty(e.Value)).Remove();
                    using (var reader = xdoc.Root.CreateReader())
                    {
                        if (serializer.Deserialize(reader) is EmbarqueDadosQualidadeXML result)
                        {
                            return result;
                        }
                        else
                        {
                            throw new Exception("Falha ao converter objeto XML");
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _log.Error($"Erro na requisição HTTP: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
                return null;

            }
            catch (Exception ex)
            {
                _log.Error($"Erro genérico ao fazer requisição do embarque: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
                return null;

            }
        }

        public LoteDadosQualidadeXML GetLoteDadosQualidade(string url)
        {
            try
            {
                _log.Debug($"Enviando requisição LoteDadosQualidade: {url.ToString().Replace(Environment.NewLine, string.Empty)}");
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    response.EnsureSuccessStatusCode();
                    _log.Debug($"Recebeu resposta LoteDadosQualidade {response.ToString().Replace(Environment.NewLine, string.Empty)}");
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    _log.Debug($"Corpo da resposta LoteDadosQualidade: {responseBody.ToString().Replace(Environment.NewLine, string.Empty)}");

                    // XML handling
                    var serializer = new XmlSerializer(typeof(LoteDadosQualidadeXML));
                    XDocument xdoc = XDocument.Parse(responseBody);
                    xdoc.Descendants().Where(e => string.IsNullOrEmpty(e.Value)).Remove();
                    using (var reader = xdoc.Root.CreateReader())
                    {
                        if (serializer.Deserialize(reader) is LoteDadosQualidadeXML result)
                        {
                            return result;
                        }
                        else
                        {
                            throw new Exception("Falha ao converter objeto XML");
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _log.Error($"Erro na requisição HTTP {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
                return null;
            }
            catch (Exception ex)
            {
                _log.Error($"Erro genérico ao fazer requisição do lote: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
                return null;
            }
        }
    }
}
