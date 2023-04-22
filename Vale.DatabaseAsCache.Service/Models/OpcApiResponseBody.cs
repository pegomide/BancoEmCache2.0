using Newtonsoft.Json;
using System;

namespace Vale.GetFuseData.ApiService.Models
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
        public int FloatValue { get; set; }

        [JsonProperty("DoubleValue")]
        public int DoubleValue { get; set; }

        [JsonProperty("Int32Value")]
        public int Int32Value { get; set; }

        [JsonProperty("StringValue")]
        public string StringValue { get; set; }
    }
}
