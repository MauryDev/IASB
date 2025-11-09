namespace Web.Database.Model;

public class SabadoAluno
{
    public int Id { get; set; }
    public int SabadoId { get; set; }
    public Sabado Sabado { get; set; }
    public int PessoaId { get; set; }
    public Pessoa Pessoa { get; set; }
    public Enum.PresencaAluno Presenca { get; set; }
}
