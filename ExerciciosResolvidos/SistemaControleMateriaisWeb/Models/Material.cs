using System.ComponentModel.DataAnnotations;

namespace SistemaControleMateriaisWeb.Models;


public class Material
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve possuir no máximo 100 caracteres.")]
    public string Nome { get; set; } = "";

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    [StringLength(80, ErrorMessage = "A categoria deve possuir no máximo 80 caracteres.")]
    public string Categoria { get; set; } = "";

    [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa.")]
    public int Quantidade { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "O estoque mínimo não pode ser negativo.")]
    [Display(Name = "Estoque mínimo")]
    public int EstoqueMinimo { get; set; }

}