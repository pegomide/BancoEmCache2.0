using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Vale.DatabaseAsCache.Data.TableModels;

namespace Vale.GetFuseData.ApiService.Models
{
    public class FuseApiRequestBody
    {
        [JsonProperty("increment")]
        public IncrementData Increment { get; set; }

        public class IncrementData
        {
            [JsonProperty("portCode")]
            public string PortCode { get; set; }

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

        public FuseApiRequestBody(ColetaFuseData data)
        {
            Increment = new IncrementData()
            {
                PortCode = "GB",
                BoardingCode = data.BOARDING_CODE,
                ProductCode = data.PRODUCT_CODE,
                ClientCode = data.CLIENT_CODE,
                PierCode = data.PIER_CODE,
                StackerName = data.STACKER_NAME,
                BoardingLine = (int)data.BOARDING_LINE.Value,
                EstimatedWheight = (double)data.ESTIMATED_WEIGHT.Value,
                WheightAtCut = (double)data.WEIGHTATCUT.Value,
                PartialSample = data.PARTIAL_SAMPLE,
                SubPartialSample = data.SUBPARTIAL_SAMPLE,
                SubSubPartialSample = data.SUBSUBPARTIAL_SAMPLE,
                IncrementNumber = data.INCREMENT_NUMBER,
                IncrementDateTime = data.INCREMENT_DATETIME.Value,
                BasementList = new List<IncrementData.BasementListData>() { }
            };

            if (data.PORAO1_ID.HasValue)
            {
                var porao1 = new FuseApiRequestBody.IncrementData.BasementListData
                {
                    BasementId = data.PORAO1_ID.Value.ToString()
                };
                if (data.PORAO1_PESO1.HasValue) { porao1.WheightPrimary = (double)data.PORAO1_PESO1.Value; }
                if (data.PORAO1_PESO2.HasValue) { porao1.WheightSecondary = (double)data.PORAO1_PESO2.Value; }
                Increment.BasementList.Add(porao1);
            }

            if (data.PORAO2_ID.HasValue)
            {
                var porao2 = new FuseApiRequestBody.IncrementData.BasementListData()
                {
                    BasementId = data.PORAO2_ID.Value.ToString(),
                };
                if (data.PORAO2_PESO1.HasValue) { porao2.WheightPrimary = (double)data.PORAO2_PESO1.Value; }
                if (data.PORAO2_PESO2.HasValue) { porao2.WheightSecondary = (double)data.PORAO2_PESO2.Value; }

                Increment.BasementList.Add(porao2);
            }
            if (data.PORAO3_ID.HasValue)
            {
                var porao3 = new FuseApiRequestBody.IncrementData.BasementListData()
                {
                    BasementId = data.PORAO3_ID.Value.ToString(),
                };
                if (data.PORAO3_PESO1.HasValue) { porao3.WheightPrimary = (double)data.PORAO3_PESO1.Value; }
                if (data.PORAO3_PESO2.HasValue) { porao3.WheightSecondary = (double)data.PORAO3_PESO2.Value; }
                Increment.BasementList.Add(porao3);
            }

        }
        public override string ToString()
        {
            var jsonSettings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz"
            };
            return JsonConvert.SerializeObject(this, jsonSettings);
        }

    }
}
