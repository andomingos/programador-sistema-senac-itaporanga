using System.ComponentModel.DataAnnotations;

namespace SistemaControleMateriaisWeb.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Informe a senha.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = "";

    [Display(Name = "Manter conectado")]
    public bool LembrarMe { get; set; }
}
