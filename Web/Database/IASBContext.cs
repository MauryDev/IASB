using Microsoft.EntityFrameworkCore;
using Web.Database.Model;

namespace Web.Database;

public class IASBContext : DbContext
{
    protected readonly IConfiguration Configuration;
    public DbSet<Model.Pessoa> Pessoas { get; set; }
    public DbSet<Model.Pergunta> Perguntas { get; set; }
    public DbSet<Model.Sabado> Sabados { get; set; }
    public DbSet<Model.SabadoAluno> SabadoAlunos { get; set; }

    public IASBContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pergunta>().HasData(
            new Pergunta { Id = 1, Ordem = 1, Texto = "NÚMERO DE ALUNOS PRESENTES:" },
            new Pergunta { Id = 2, Ordem = 2, Texto = "QUANTOS ESTUDARAM DIARIAMENTE A BÍBLIA E A LIÇÃO?" },
            new Pergunta { Id = 3, Ordem = 3, Texto = "QUANTOS PARTICIPARAM DE UM PEQUENO GRUPO NESTA SEMANA?" },
            new Pergunta { Id = 4, Ordem = 4, Texto = "QUANTOS DERAM ALGUM ESTUDO BÍBLICO NESTA SEMANA?" },
            new Pergunta { Id = 5, Ordem = 5, Texto = "QUANTOS REALIZARAM ALGUMA OUTRA ATIVIDADE MISSIONÁRIA UTILIZANDO SEU TALENTO?" }
        );
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Configuration.GetConnectionString("IASB_DB"));
    }
}
