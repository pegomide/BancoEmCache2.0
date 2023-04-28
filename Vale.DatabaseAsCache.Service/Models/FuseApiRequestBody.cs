using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Vale.GetFuseData.ApiService.Models
{
    public class FuseApiRequestBody
    {
        [JsonProperty("increment")]
        public IncrementData Increment { get; set; }

        public class IncrementData
        {
            [JsonProperty("boardingCode")]
            public string BoardingCode { get; set; }

            [JsonProperty("productCode")]
            public string ProductCode { get; set; }

            [JsonProperty("clientCode")]
            public string ClientCode { get; set; }

            [JsonProperty("pierCode")]
            public string PierCode { get; set; }

            [JsonProperty("stackerName")]
            public string StackerName { get; set; }

            [JsonProperty("boardingLine")]
            public int BoardingLine { get; set; }

            [JsonProperty("estimatedWheight")]
            public double EstimatedWheight { get; set; }

            [JsonProperty("wheightAtCut")]
            public double WheightAtCut { get; set; }

            [JsonProperty("partialSample")]
            public string PartialSample { get; set; }

            [JsonProperty("subPartialSample")]
            public string SubPartialSample { get; set; }

            [JsonProperty("subSubPartialSample")]
            public string SubSubPartialSample { get; set; }

            [JsonProperty("incrementNumber")]
            public int IncrementNumber { get; set; }

            [JsonProperty("incrementDateTime")]
            public DateTime IncrementDateTime { get; set; }

            [JsonProperty("basementList")]
            public List<BasementListData> BasementList { get; set; }

            public class BasementListData
            {
                [JsonProperty("basement")]
                public string BasementId { get; set; }

                [JsonProperty("wheightPrimary")]
                public double WheightPrimary { get; set; }

                [JsonProperty("wheightSecondary")]
                public double WheightSecondary { get; set; }
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
