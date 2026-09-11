using Microsoft.Data.Sqlite;
using SistemaControleMateriaisWeb.Data;
using SistemaControleMateriaisWeb.Models;

namespace SistemaControleMateriaisWeb.Repositories;

public class MaterialRepository
{
    private readonly DatabaseConnectionFactory _factory;

    public MaterialRepository(DatabaseConnectionFactory factory)
    {
        _factory = factory;
    }

    public List<Material> ListarTodos()
    {
        var materiais = new List<Material>();

        using var conexao = _factory.CriarConexao();
        conexao.Open();

        const string sql = @"
            SELECT Id, Nome, Categoria, Quantidade, EstoqueMinimo
            FROM Materiais
            ORDER BY Nome;";

        using var comando = new SqliteCommand(sql, conexao);
        using var leitor = comando.ExecuteReader();

        while (leitor.Read())
        {
            materiais.Add(MapearMaterial(leitor));
        }

        return materiais;
    }

    public Material? BuscarPorId(int id)
    {
        using var conexao = _factory.CriarConexao();
        conexao.Open();

        const string sql = @"
            SELECT Id, Nome, Categoria, Quantidade, EstoqueMinimo
            FROM Materiais
            WHERE Id = @Id;";

        using var comando = new SqliteCommand(sql, conexao);
        comando.Parameters.AddWithValue("@Id", id);

        using var leitor = comando.ExecuteReader();

        if (leitor.Read())
            return MapearMaterial(leitor);

        return null;
    }

    public void Inserir(Material material)
    {
        using var conexao = _factory.CriarConexao();
        conexao.Open();

        const string sql = @"
            INSERT INTO Materiais
                (Nome, Categoria, Quantidade, EstoqueMinimo)
            VALUES
                (@Nome, @Categoria, @Quantidade, @EstoqueMinimo);";

        using var comando = new SqliteCommand(sql, conexao);
        comando.Parameters.AddWithValue("@Nome", material.Nome.Trim());
        comando.Parameters.AddWithValue("@Categoria", material.Categoria.Trim());
        comando.Parameters.AddWithValue("@Quantidade", material.Quantidade);
        comando.Parameters.AddWithValue("@EstoqueMinimo", material.EstoqueMinimo);
        comando.ExecuteNonQuery();
    }

    public bool Atualizar(Material material)
    {
        using var conexao = _factory.CriarConexao();
        conexao.Open();

        const string sql = @"
            UPDATE Materiais
            SET Nome = @Nome,
                Categoria = @Categoria,
                Quantidade = @Quantidade,
                EstoqueMinimo = @EstoqueMinimo
            WHERE Id = @Id;";

        using var comando = new SqliteCommand(sql, conexao);
        comando.Parameters.AddWithValue("@Nome", material.Nome.Trim());
        comando.Parameters.AddWithValue("@Categoria", material.Categoria.Trim());
        comando.Parameters.AddWithValue("@Quantidade", material.Quantidade);
        comando.Parameters.AddWithValue("@EstoqueMinimo", material.EstoqueMinimo);
        comando.Parameters.AddWithValue("@Id", material.Id);

        return comando.ExecuteNonQuery() > 0;
    }

    public bool Excluir(int id)
    {
        using var conexao = _factory.CriarConexao();
        conexao.Open();

        const string sql = "DELETE FROM Materiais WHERE Id = @Id;";

        using var comando = new SqliteCommand(sql, conexao);
        comando.Parameters.AddWithValue("@Id", id);

        return comando.ExecuteNonQuery() > 0;
    }

    private static Material MapearMaterial(SqliteDataReader leitor)
    {
        return new Material
        {
            Id = leitor.GetInt32(0),
            Nome = leitor.GetString(1),
            Categoria = leitor.GetString(2),
            Quantidade = leitor.GetInt32(3),
            EstoqueMinimo = leitor.GetInt32(4)
        };
    }
}