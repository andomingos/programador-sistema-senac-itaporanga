using Microsoft.Data.Sqlite;
using SistemaControleMateriaisWeb.Data;
using SistemaControleMateriaisWeb.Models;

namespace SistemaControleMateriaisWeb.Repositories;

public class UsuarioRepository
{
    private readonly DatabaseConnectionFactory _factory;

    public UsuarioRepository(DatabaseConnectionFactory factory)
    {
        _factory = factory;
    }

    public bool ExisteAlgumUsuario()
    {
        using var conexao = _factory.CriarConexao();
        conexao.Open();

        const string sql = "SELECT COUNT(*) FROM Usuarios;";

        using var comando = new SqliteCommand(sql, conexao);

        long quantidade =
            (long)(comando.ExecuteScalar() ?? 0L);

        return quantidade > 0;
    }

    public Usuario? BuscarPorEmail(string email)
    {
        using var conexao = _factory.CriarConexao();
        conexao.Open();

        const string sql = """
            SELECT
                Id,
                Nome,
                Email,
                SenhaHash,
                Ativo,
                CriadoEm,
                Perfil
            FROM Usuarios
            WHERE Email = @Email
            LIMIT 1;
            """;

        using var comando =
            new SqliteCommand(sql, conexao);

        comando.Parameters.AddWithValue(
            "@Email",
            email.Trim());

        using var leitor =
            comando.ExecuteReader();

        if (!leitor.Read())
            return null;

        return new Usuario
        {
            Id = leitor.GetInt32(0),
            Nome = leitor.GetString(1),
            Email = leitor.GetString(2),
            SenhaHash = leitor.GetString(3),
            Ativo = leitor.GetInt32(4) == 1,
            CriadoEm = DateTime.Parse(
                leitor.GetString(5)),
            Perfil = leitor.GetString(6)
        };
    }

    public Usuario? BuscarPorId(int id)
    {
        using var conexao = _factory.CriarConexao();
        conexao.Open();

        const string sql = """
            SELECT
                Id,
                Nome,
                Email,
                SenhaHash,
                Ativo,
                CriadoEm,
                Perfil
            FROM Usuarios
            WHERE Id = @Id
            LIMIT 1;
            """;

        using var comando =
            new SqliteCommand(sql, conexao);

        comando.Parameters.AddWithValue(
            "@Id",
            id);

        using var leitor =
            comando.ExecuteReader();

        if (!leitor.Read())
            return null;

        return new Usuario
        {
            Id = leitor.GetInt32(0),
            Nome = leitor.GetString(1),
            Email = leitor.GetString(2),
            SenhaHash = leitor.GetString(3),
            Ativo = leitor.GetInt32(4) == 1,
            CriadoEm = DateTime.Parse(
                leitor.GetString(5)),
            Perfil = leitor.GetString(6)
        };
    }

    public List<Usuario> ListarTodos()
    {
        var usuarios = new List<Usuario>();

        using var conexao = _factory.CriarConexao();
        conexao.Open();

        const string sql = """
            SELECT
                Id,
                Nome,
                Email,
                SenhaHash,
                Ativo,
                CriadoEm,
                Perfil
            FROM Usuarios
            ORDER BY Nome;
            """;

        using var comando =
            new SqliteCommand(sql, conexao);

        using var leitor =
            comando.ExecuteReader();

        while (leitor.Read())
        {
            usuarios.Add(
                new Usuario
                {
                    Id = leitor.GetInt32(0),
                    Nome = leitor.GetString(1),
                    Email = leitor.GetString(2),
                    SenhaHash = leitor.GetString(3),
                    Ativo = leitor.GetInt32(4) == 1,
                    CriadoEm = DateTime.Parse(
                        leitor.GetString(5)),
                    Perfil = leitor.GetString(6)
                });
        }

        return usuarios;
    }

    public void Inserir(Usuario usuario)
    {
        using var conexao = _factory.CriarConexao();
        conexao.Open();

        const string sql = """
            INSERT INTO Usuarios
            (
                Nome,
                Email,
                SenhaHash,
                Ativo,
                CriadoEm,
                Perfil
            )
            VALUES
            (
                @Nome,
                @Email,
                @SenhaHash,
                @Ativo,
                @CriadoEm,
                @Perfil
            );
            """;

        using var comando =
            new SqliteCommand(sql, conexao);

        comando.Parameters.AddWithValue(
            "@Nome",
            usuario.Nome);

        comando.Parameters.AddWithValue(
            "@Email",
            usuario.Email);

        comando.Parameters.AddWithValue(
            "@SenhaHash",
            usuario.SenhaHash);

        comando.Parameters.AddWithValue(
            "@Ativo",
            usuario.Ativo ? 1 : 0);

        comando.Parameters.AddWithValue(
            "@CriadoEm",
            usuario.CriadoEm.ToString(
                "yyyy-MM-dd HH:mm:ss"));

        comando.Parameters.AddWithValue(
            "@Perfil",
            usuario.Perfil);

        comando.ExecuteNonQuery();
    }

    public void Atualizar(Usuario usuario)
    {
        using var conexao = _factory.CriarConexao();
        conexao.Open();

        const string sql = """
            UPDATE Usuarios
            SET
                Nome = @Nome,
                Email = @Email,
                SenhaHash = @SenhaHash,
                Ativo = @Ativo,
                Perfil = @Perfil
            WHERE Id = @Id;
            """;

        using var comando =
            new SqliteCommand(sql, conexao);

        comando.Parameters.AddWithValue(
            "@Nome",
            usuario.Nome);

        comando.Parameters.AddWithValue(
            "@Email",
            usuario.Email);

        comando.Parameters.AddWithValue(
            "@SenhaHash",
            usuario.SenhaHash);

        comando.Parameters.AddWithValue(
            "@Ativo",
            usuario.Ativo ? 1 : 0);

        comando.Parameters.AddWithValue(
            "@Perfil",
            usuario.Perfil);

        comando.Parameters.AddWithValue(
            "@Id",
            usuario.Id);

        comando.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var conexao = _factory.CriarConexao();
        conexao.Open();

        const string sql = """
            DELETE FROM Usuarios
            WHERE Id = @Id;
            """;

        using var comando =
            new SqliteCommand(sql, conexao);

        comando.Parameters.AddWithValue(
            "@Id",
            id);

        comando.ExecuteNonQuery();
    }
}