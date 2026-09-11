using Microsoft.Data.Sqlite;

namespace SistemaControleMateriaisWeb.Data;

public class DatabaseConnectionFactory
{
    private readonly string _connectionString;

    public DatabaseConnectionFactory(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("SistemaControleMateriais")
            ?? "Data Source=sistema-controle-materiais.db";
    }

    public SqliteConnection CriarConexao()
    {
        return new SqliteConnection(_connectionString);
    }
}
