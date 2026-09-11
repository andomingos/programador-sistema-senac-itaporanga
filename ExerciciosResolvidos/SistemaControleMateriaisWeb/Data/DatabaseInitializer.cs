using Microsoft.AspNetCore.Identity;
using SistemaControleMateriaisWeb.Models;
using SistemaControleMateriaisWeb.Repositories;

namespace SistemaControleMateriaisWeb.Data;

public class DatabaseInitializer
{
    private readonly DatabaseConnectionFactory _factory;
    private readonly UsuarioRepository _usuarios;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public DatabaseInitializer(
        DatabaseConnectionFactory factory,
        UsuarioRepository usuarios,
        IPasswordHasher<Usuario> passwordHasher)
    {
        _factory = factory;
        _usuarios = usuarios;
        _passwordHasher = passwordHasher;
    }

    public void Inicializar()
    {
        CriarTabelas();
        CriarAdministradorInicial();
    }

    private void CriarTabelas()
    {
        using var conexao = _factory.CriarConexao();
        conexao.Open();

        string sql = @"
            CREATE TABLE IF NOT EXISTS Materiais
            (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                Categoria TEXT NOT NULL,
                Quantidade INTEGER NOT NULL,
                EstoqueMinimo INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Usuarios
            (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                Email TEXT NOT NULL COLLATE NOCASE UNIQUE,
                SenhaHash TEXT NOT NULL,
                Ativo INTEGER NOT NULL DEFAULT 1,
                CriadoEm TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            );";

        using var comando = conexao.CreateCommand();
        comando.CommandText = sql;
        comando.ExecuteNonQuery();
    }

    private void CriarAdministradorInicial()
    {
        if (_usuarios.ExisteAlgumUsuario())
            return;

        string nome =
            Environment.GetEnvironmentVariable("ADMIN_NOME")
            ?? "Administrador";

        string? email =
            Environment.GetEnvironmentVariable("ADMIN_EMAIL");

        string? senha =
            Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(senha))
        {
            Console.WriteLine(
                "Usuário inicial não criado. Defina ADMIN_EMAIL e ADMIN_PASSWORD antes da primeira execução.");
            return;
        }

        var usuario = new Usuario
        {
            Nome = nome.Trim(),
            Email = email.Trim(),
            Ativo = true,
            CriadoEm = DateTime.UtcNow
        };

        usuario.SenhaHash =
            _passwordHasher.HashPassword(usuario, senha);

        _usuarios.Inserir(usuario);

        Console.WriteLine(
            $"Usuário administrador inicial criado: {usuario.Email}");
    }
}