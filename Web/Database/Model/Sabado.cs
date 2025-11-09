namespace Web.Database.Model;

public class Sabado : ICloneable
{
    public int Id { get; set; }
    public int Ano { get; set; }
    public int Trimestre { get; set; }
    public int Index { get; set; }
    public int AlunosPresentes { get; set; }
    public int EstudoDiarioBibliaLicao { get; set; }
    public int ParticipacaoPequenoGrupo { get; set; }
    public int EstudosBiblicosDados { get; set; }
    public int OutrasAtividadesMissionarias { get; set; }
    public object Clone()
    {
        return this.MemberwiseClone();
    }

}
