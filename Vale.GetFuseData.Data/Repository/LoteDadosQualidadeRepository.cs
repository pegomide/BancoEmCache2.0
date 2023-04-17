using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Vale.GetFuseData.Data.TableModels;

namespace Vale.GetFuseData.Data.Repository
{
    public interface ILoteDadosQualidadeRepository
    {
        bool IsConnectionOpen();
        Task<bool> Exists(LoteDadosQualidade embarqueDados);
        Task<int> Insert(LoteDadosQualidade embarqueDados);
        Task<int> Update(LoteDadosQualidade embarqueDados);
        Task<int> ClearTable();
    }

    public class LoteDadosQualidadeRepository : ILoteDadosQualidadeRepository
    {
        private readonly string _connectionString;
        private readonly string _tableName = "LoteDadosQualidade";

        public LoteDadosQualidadeRepository(string connectionString)
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
        /// Verifica se já existe a linha com os dados enviados na tabela LoteDadosQualidade.
        /// </summary>
        /// <param name="embarqueDados">Objeto cujos dados serão consultados no banco.</param>
        /// <returns>Verdadeiro se já contém essa linha.</returns>
        public async Task<bool> Exists(LoteDadosQualidade loteDados)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var sql = $@"
                IF EXISTS(
                    SELECT * FROM [dbo].[{_tableName}] 
                    WHERE 
                        GpvLoteId = '{loteDados.GpvLoteId}' AND 
                        MatriculaPrimeiroVagao = '{loteDados.MatriculaPrimeiroVagao}' AND 
                        PrefixoTrem = '{loteDados.PrefixoTrem}' AND
                        Produto = '{loteDados.Produto}')
                    SELECT CAST(1 AS BIT)
                ELSE
                    SELECT CAST(0 AS BIT)
                ";
                return await connection.QueryFirstAsync<bool>(sql, loteDados);
            }
        }

        /// <summary>
        /// Salva informações enviadas em uma nova linha na tabela LoteDadosQualidade.
        /// </summary>
        /// <param name="embarqueDados">Dados a serem salvos na tabela.</param>
        /// <returns>Quantidade de registros salvos.</returns>
        public async Task<int> Insert(LoteDadosQualidade loteDados)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $@"
                INSERT [dbo].[{_tableName}] (
                        [GpvLoteId], 
                        [MatriculaPrimeiroVagao], 
                        [PrefixoTrem], 
                        [PrevisaoChegada], 
                        [DataHoraRegistro], 
                        [Produto], 
                        [VagoesProgramados], 
                        [TotalProgramado],
                        [MatriculaUltimoVagao]
                    ) VALUES (
                        '{loteDados.GpvLoteId}',
                        '{loteDados.MatriculaPrimeiroVagao}',
                        '{loteDados.PrefixoTrem}',
                        CAST(N'{loteDados.PrevisaoChegada:yyyy-MM-dd HH:mm:ss.fff}' AS DateTime),
                        CAST(N'{loteDados.DataHoraRegistro:yyyy-MM-dd HH:mm:ss.fff}' AS DateTime),
                        '{loteDados.Produto}', 
                        {loteDados.VagoesProgramados}, 
                        {loteDados.TotalProgramado.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)},
                        '{loteDados.MatriculaUltimoVagao}'
                    )
                ";
                return await connection.ExecuteAsync(sql, loteDados);
            }
        }

        /// <summary>
        /// Atualiza dado conforme colunas 'PrevisaoChegada','DataHoraRegistro','VagoesProgramados','TotalProgramado' e 'MatriculaUltimoVagao' na tabela LoteDadosQualidade.
        /// </summary>
        /// <param name="embarqueDados">Dados a serem atualizados.</param>
        /// <returns>Quantidade de linhas atualizadas.</returns>
        public async Task<int> Update(LoteDadosQualidade loteDados)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var sql = $@"
                UPDATE [dbo].[{_tableName}]
                SET
                    [PrevisaoChegada] = CAST(N'{loteDados.PrevisaoChegada:yyyy-MM-dd HH:mm:ss.fff}' AS DateTime),
                    [DataHoraRegistro] = CAST(N'{loteDados.DataHoraRegistro:yyyy-MM-dd HH:mm:ss.fff}' AS DateTime),
                    [VagoesProgramados] = {loteDados.VagoesProgramados}, 
                    [TotalProgramado] = {loteDados.TotalProgramado.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)},
                    [MatriculaUltimoVagao] = '{loteDados.MatriculaUltimoVagao}'
                WHERE 
                    GpvLoteId = '{loteDados.GpvLoteId}' AND 
                    MatriculaPrimeiroVagao = '{loteDados.MatriculaPrimeiroVagao}' AND 
                    PrefixoTrem = '{loteDados.PrefixoTrem}' AND
                    Produto = '{loteDados.Produto}'
                ";
                return await connection.ExecuteAsync(sql, loteDados);
            }
        }

        /// <summary>
        /// Deleta todos dados da tabela LoteDadosQualidade.
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
