using log4net;
using System;
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
                FuseApiRequestBody body = new FuseApiRequestBody(data) { };
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
