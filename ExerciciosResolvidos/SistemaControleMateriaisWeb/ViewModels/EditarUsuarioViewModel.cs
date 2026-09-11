using System.ComponentModel.DataAnnotations;

namespace SistemaControleMateriaisWeb.ViewModels;

public class EditarUsuarioViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome.")]
    public string Nome { get; set; } = "";

    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = "";

    public bool Ativo { get; set; }

    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    public string? NovaSenha { get; set; }

    [DataType(DataType.Password)]
    [Compare("NovaSenha", ErrorMessage = "As senhas não conferem.")]
    [Display(Name = "Confirmar nova senha")]
    public string? ConfirmarNovaSenha { get; set; }
}