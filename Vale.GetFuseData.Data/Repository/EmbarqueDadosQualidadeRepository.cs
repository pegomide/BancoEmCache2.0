using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Vale.GetFuseData.Data.TableModels;

namespace Vale.GetFuseData.Data.Repository
{
    public interface IEmbarqueDadosQualidadeRepository
    {
        bool IsConnectionOpen();
        Task<bool> Exists(EmbarqueDadosQualidade embarqueDados);
        Task<int> Insert(EmbarqueDadosQualidade embarqueDados);
        Task<int> Update(EmbarqueDadosQualidade embarqueDados);
        Task<int> ClearTable();
    }
    public class EmbarqueDadosQualidadeRepository : IEmbarqueDadosQualidadeRepository
    {

        private readonly string _connectionString;
        private readonly string _tableName = "EmbarqueDadosQualidade";

        public EmbarqueDadosQualidadeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Valida a capacidade de conexão com o banco.
        /// </summary>
        /// <returns>Se a conexão foi aberta.</returns>
        public bool IsConnectionOpen()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                bool isOpen = connection.State.Equals(ConnectionState.Open);
                connection.Close();

                return isOpen;
            }
        }

        /// <summary>
        /// Verifica se já existe a linha com os dados enviados na tabela EmbarqueDadosQualidade.
        /// </summary>
        /// <param name="embarqueDados">Objeto cujos dados serão consultados no banco.</param>
        /// <returns>Verdadeiro se já contém essa linha.</returns>
        public async Task<bool> Exists(EmbarqueDadosQualidade embarqueDados)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $@"
                IF EXISTS(
                    SELECT 1 FROM [dbo].[{_tableName}] 
                    WHERE 
                        GpvEmbarqueId = '{embarqueDados.GpvEmbarqueId}' AND 
                        NomeNavio = '{embarqueDados.NomeNavio}')
                    SELECT CAST(1 AS BIT)
                ELSE
                    SELECT CAST(0 AS BIT)
                ";
                return await connection.QueryFirstAsync<bool>(sql, embarqueDados);
            }
        }

        /// <summary>
        /// Salva informações enviadas em uma nova linha na tabela EmbarqueDadosQualidade.
        /// </summary>
        /// <param name="embarqueDados">Dados a serem salvos na tabela.</param>
        /// <returns>Quantidade de registros salvos.</returns>
        public async Task<int> Insert(EmbarqueDadosQualidade embarqueDados)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $@"
                INSERT [dbo].[{_tableName}] (
                    [GpvEmbarqueId], 
                    [NomeNavio], 
                    [Pier], 
                    [PrevisaoAtracacao], 
                    [DataHoraRegistro], 
                    [Produto], 
                    [AmostrasProgramadas], 
                    [CargaTotal]
                ) VALUES (
                    '{embarqueDados.GpvEmbarqueId}',
                    '{embarqueDados.NomeNavio}',
                    '{embarqueDados.Pier}',
                    CAST(N'{embarqueDados.PrevisaoAtracacao:yyyy-MM-dd HH:mm:ss.fff}' AS DateTime),
                    CAST(N'{embarqueDados.DataHoraRegistro:yyyy-MM-dd HH:mm:ss.fff}' AS DateTime),
                    '{embarqueDados.Produto}', 
                    {embarqueDados.AmostrasProgramadas}, 
                    {embarqueDados.CargaTotal}
                )";
                return await connection.ExecuteAsync(sql, embarqueDados);
            }
        }

        /// <summary>
        /// Atualiza dado conforme colunas 'GpvEmbarqueId','NomeNavio' e 'Produto' na tabela EmbarqueDadosQualidade.
        /// </summary>
        /// <param name="embarqueDados">Dados a serem atualizados.</param>
        /// <returns>Quantidade de linhas atualizadas.</returns>
        public async Task<int> Update(EmbarqueDadosQualidade embarqueDados)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $@"
                UPDATE [dbo].[{_tableName}]
                SET
                    [Pier] = '{embarqueDados.Pier}',
                    [PrevisaoAtracacao] = CAST(N'{embarqueDados.PrevisaoAtracacao:yyyy-MM-dd HH:mm:ss.fff}' AS DateTime), 
                    [DataHoraRegistro] = CAST(N'{embarqueDados.DataHoraRegistro:yyyy-MM-dd HH:mm:ss.fff}' AS DateTime), 
                    [AmostrasProgramadas] = {embarqueDados.AmostrasProgramadas}, 
                    [CargaTotal] = {embarqueDados.CargaTotal}
                WHERE 
                    GpvEmbarqueId = '{embarqueDados.GpvEmbarqueId}' AND 
                    NomeNavio = '{embarqueDados.NomeNavio}' AND 
                    Produto = '{embarqueDados.Produto}'
                ";
                return await connection.ExecuteAsync(sql, embarqueDados);
            }
        }

        /// <summary>
        /// Deleta todos dados da tabela EmbarqueDadosQualidade.
        /// </summary>
        /// <returns>Quantidade de linhas afetadas.</returns>
        public async Task<int> ClearTable()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"DELETE FROM [dbo].[{_tableName}]";
                return await connection.ExecuteAsync(sql);
            }
        }
    }
}
