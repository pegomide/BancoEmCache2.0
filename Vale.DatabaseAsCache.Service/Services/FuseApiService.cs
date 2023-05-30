using log4net;
using System;
using System.Collections.Generic;
using Vale.DatabaseAsCache.Data.TableModels;
using Vale.GetFuseData.ApiService.Models;

namespace Vale.GetFuseData.ApiService.Services
{
    public static class FuseApiService
    {
        private static readonly ILog _log = LogManager.GetLogger("log");

        public static FuseApiRequestBody TransformDatabaseIntoRequestBody(ColetaFuseData data)
        {
            try
            {
                FuseApiRequestBody body = new FuseApiRequestBody()
                {
                    Increment = new FuseApiRequestBody.IncrementData()
                    {
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
                        IncrementDateTime = data.INCREMENT_DATETIME.Value
                    }
                };
                var basementList = new List<FuseApiRequestBody.IncrementData.BasementListData>() { };
                if (data.PORAO1_ID.HasValue)
                {
                    var porao1 = new FuseApiRequestBody.IncrementData.BasementListData()
                    {
                        BasementId = data.PORAO1_ID.Value.ToString()
                    };
                    if (data.PORAO1_PESO1.HasValue) { porao1.WheightPrimary = (double)data.PORAO1_PESO1.Value; }
                    if (data.PORAO1_PESO2.HasValue) { porao1.WheightSecondary = (double)data.PORAO1_PESO2.Value; }
                    basementList.Add(porao1);
                }

                if (data.PORAO2_ID.HasValue)
                {
                    var porao2 = new FuseApiRequestBody.IncrementData.BasementListData()
                    {
                        BasementId = data.PORAO2_ID.Value.ToString(),
                    };
                    if (data.PORAO2_PESO1.HasValue) { porao2.WheightPrimary = (double)data.PORAO2_PESO1.Value; }
                    if (data.PORAO2_PESO2.HasValue) { porao2.WheightSecondary = (double)data.PORAO2_PESO2.Value; }

                    basementList.Add(porao2);
                }
                if (data.PORAO3_ID.HasValue)
                {
                    var porao3 = new FuseApiRequestBody.IncrementData.BasementListData()
                    {
                        BasementId = data.PORAO3_ID.Value.ToString(),
                    };
                    if (data.PORAO3_PESO1.HasValue) { porao3.WheightPrimary = (double)data.PORAO3_PESO1.Value; }
                    if (data.PORAO3_PESO2.HasValue) { porao3.WheightSecondary = (double)data.PORAO3_PESO2.Value; }
                    basementList.Add(porao3);
                }

                body.Increment.BasementList = basementList;

                return body;
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao converter dado do banco para montar requisição do Fuse:", ex);
                return null;
            }
        }
    }
}
