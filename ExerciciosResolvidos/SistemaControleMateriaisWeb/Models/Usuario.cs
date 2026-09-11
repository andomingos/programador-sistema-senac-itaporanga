namespace SistemaControleMateriaisWeb.Models;

public class Usuario
{
    public int Id { get; set; }

    public string Nome { get; set; } = "";

    public string Email { get; set; } = "";

    public string SenhaHash { get; set; } = "";

    public bool Ativo { get; set; }

    public DateTime CriadoEm { get; set; }

    public string Perfil { get; set; } = "Usuario";
}