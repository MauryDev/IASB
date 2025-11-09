using System.ComponentModel.DataAnnotations;

namespace Web.View.EscolaSabatina;

public class PessoaCreate
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(200, ErrorMessage = "O nome pode ter no máximo {1} caracteres.")]
    public string? Nome { get; set; }

    public bool Ativo { get; set; } = true;
}
