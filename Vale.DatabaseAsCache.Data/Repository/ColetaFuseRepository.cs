using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Vale.GetFuseData.Data.TableModels;

namespace Vale.GetFuseData.Data.Repository
{
    public interface IColetaFuseRepository
    {
        Task<IEnumerable<ColetaFuseData>> Select(ColetaFuseData data);
        Task<int> Insert(ColetaFuseData data);

    }

    public class ColetaFuseRepository : IColetaFuseRepository
    {
        private readonly string _tableName = "ColetaFuse";
        private readonly IDbConnection _connection;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="connectionString">Database connection string.</param>
        public ColetaFuseRepository(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Select data based on filter sent.
        /// </summary>
        /// <param name="data">Use as filter</param>
        /// <returns>List of data searched.</returns>
        public async Task<IEnumerable<ColetaFuseData>> Select(ColetaFuseData data)
        {
            var sql = $@"
                SELECT [PIER_CODE]
                      ,[BOARDING_CODE]
                      ,[INCREMENT_NUMBER]
                      ,[PRODUCT_CODE]
                      ,[CLIENT_CODE]
                      ,[STACKER_NAME]
                      ,[ESTIMATED_WEIGHT]
                      ,[PORAO1_ID]
                      ,[PORAO1_PESO]
                      ,[PORAO2_ID]
                      ,[PORAO2_PESO]
                      ,[PORAO3_ID]
                      ,[PORAO3_PESO]
                      ,[ORDER_NUMBER]
                      ,[BOARDING_LINE]
                      ,[INCREMENT_DATETIME]
                      ,[PARTIAL_SAMPLE]
                      ,[SUBPARTIAL_SAMPLE]
                      ,[SUBSUBPARTIAL_SAMPLE]
                      ,[WEIGHTATCUT]
                      ,[STATUS_TYPE]
                  FROM [dbo].[{_tableName}]
                  WHERE [PIER_CODE] = '{data.PIER_CODE}'
                      ,[BOARDING_CODE] = '{data.BOARDING_CODE}'
                      ,[INCREMENT_NUMBER] = {data.INCREMENT_NUMBER}
            ";
            return await _connection.QueryAsync<ColetaFuseData>(sql, data);
        }

        /// <summary>
        /// Insert new line into table
        /// </summary>
        /// <param name="data">Objeto com dados a serem inseridos.</param>
        /// <returns>Number of lines inserted. Probably one.</returns>
        public async Task<int> Insert(ColetaFuseData data)
        {
            var sql = $@"
                INSERT INTO [dbo].[{_tableName}]
                    ([PRODUCT_CODE]
                    ,[CLIENT_CODE]
                    ,[PIER_CODE]
                    ,[BOARDING_CODE]
                    ,[STACKER_NAME]
                    ,[ESTIMATED_WEIGHT]
                    ,[PORAO1_ID]
                    ,[PORAO1_PESO]
                    ,[PORAO2_ID]
                    ,[PORAO2_PESO]
                    ,[PORAO3_ID]
                    ,[PORAO3_PESO]
                    ,[ORDER_NUMBER]
                    ,[BOARDING_LINE]
                    ,[INCREMENT_DATETIME]
                    ,[PARTIAL_SAMPLE]
                    ,[SUBPARTIAL_SAMPLE]
                    ,[SUBSUBPARTIAL_SAMPLE]
                    ,[INCREMENT_NUMBER]
                    ,[WEIGHTATCUT]
                    ,[STATUS_TYPE])
                VALUES
                    ({data.PRODUCT_CODE}
                    ,{data.CLIENT_CODE}
                    ,{data.PIER_CODE}
                    ,{data.BOARDING_CODE}
                    ,{data.STACKER_NAME}
                    ,{data.ESTIMATED_WEIGHT}
                    ,{data.PORAO1_ID}
                    ,{data.PORAO1_PESO}
                    ,{data.PORAO2_ID}
                    ,{data.PORAO2_PESO}
                    ,{data.PORAO3_ID}
                    ,{data.PORAO3_PESO}
                    ,{data.ORDER_NUMBER}
                    ,{data.BOARDING_LINE}
                    ,{data.INCREMENT_DATETIME}
                    ,{data.PARTIAL_SAMPLE}
                    ,{data.SUBPARTIAL_SAMPLE}
                    ,{data.SUBSUBPARTIAL_SAMPLE}
                    ,{data.INCREMENT_NUMBER}
                    ,{data.WEIGHTATCUT}
                    ,{data.STATUS_TYPE})
            ";
            return await _connection.ExecuteAsync(sql, data);
        }
    }
}
