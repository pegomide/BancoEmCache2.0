using Newtonsoft.Json;
using System;

namespace Vale.DatabaseAsCache.ApiService.Models
{
    public class OpcApiResponseBody
    {
        [JsonProperty("Value")]
        public object Value { get; set; }

        [JsonProperty("TimeStamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("Quality")]
        public string Quality { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("FloatValue")]
        public float FloatValue { get; set; }

        [JsonProperty("DoubleValue")]
        public double DoubleValue { get; set; }

        [JsonProperty("Int32Value")]
        public int Int32Value { get; set; }

        [JsonProperty("StringValue")]
        public string StringValue { get; set; }
    }
}
