using Newtonsoft.Json;
using System.Collections.Generic;

namespace Vale.GetFuseData.ApiService.Models
{
    public class OpcApiRequestBody
    {
        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("servername")]
        public string Servername { get; set; }

        [JsonProperty("items")]
        public List<string> Items { get; set; }

    }
}
