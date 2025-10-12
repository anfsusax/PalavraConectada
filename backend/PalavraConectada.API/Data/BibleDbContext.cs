// DbContext - Conexão com o banco de dados
// Como o Tabernáculo era o lugar onde Deus habitava, o DbContext é onde os dados habitam
using Microsoft.EntityFrameworkCore;
using PalavraConectada.API.Models;

namespace PalavraConectada.API.Data;

public class BibleDbContext : DbContext
{
    public BibleDbContext(DbContextOptions<BibleDbContext> options) : base(options)
    {
    }

    // Tabelas do banco de dados
    public DbSet<Verse> Verses { get; set; }
    public DbSet<Emotion> Emotions { get; set; }
    public DbSet<VerseEmotion> VerseEmotions { get; set; }
    public DbSet<BibleStory> BibleStories { get; set; }
    public DbSet<UserInteraction> UserInteractions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar relacionamento muitos-para-muitos
        modelBuilder.Entity<VerseEmotion>()
            .HasKey(ve => new { ve.VerseId, ve.EmotionId });

        modelBuilder.Entity<VerseEmotion>()
            .HasOne(ve => ve.Verse)
            .WithMany(v => v.VerseEmotions)
            .HasForeignKey(ve => ve.VerseId);

        modelBuilder.Entity<VerseEmotion>()
            .HasOne(ve => ve.Emotion)
            .WithMany(e => e.VerseEmotions)
            .HasForeignKey(ve => ve.EmotionId);

        // Índices para performance
        modelBuilder.Entity<Verse>()
            .HasIndex(v => v.BookAbbrev);

        modelBuilder.Entity<Verse>()
            .HasIndex(v => v.Version);

        modelBuilder.Entity<Verse>()
            .HasIndex(v => new { v.BookAbbrev, v.Chapter, v.Number });

        modelBuilder.Entity<Emotion>()
            .HasIndex(e => e.Name);

        // Seed inicial de emoções
        SeedEmotions(modelBuilder);
    }

    private void SeedEmotions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Emotion>().HasData(
            new Emotion
            {
                Id = 1,
                Name = "tristeza",
                Keywords = "triste,tristeza,deprimido,melancólico,abatido,desanimado",
                Description = "Sentimento de tristeza, dor emocional ou desânimo",
                RecommendationType = "consolo"
            },
            new Emotion
            {
                Id = 2,
                Name = "alegria",
                Keywords = "feliz,alegre,contente,radiante,animado,júbilo",
                Description = "Sentimento de alegria, contentamento e júbilo",
                RecommendationType = "louvor"
            },
            new Emotion
            {
                Id = 3,
                Name = "medo",
                Keywords = "medo,temor,pavor,assustado,receio,apreensivo",
                Description = "Sentimento de medo, ansiedade ou preocupação",
                RecommendationType = "coragem"
            },
            new Emotion
            {
                Id = 4,
                Name = "ansiedade",
                Keywords = "ansioso,preocupado,nervoso,tenso,estressado,angustiado",
                Description = "Sentimento de ansiedade, preocupação ou estresse",
                RecommendationType = "paz"
            },
            new Emotion
            {
                Id = 5,
                Name = "solidão",
                Keywords = "só,sozinho,solitário,isolado,abandonado,desamparado",
                Description = "Sentimento de solidão ou isolamento",
                RecommendationType = "companhia"
            },
            new Emotion
            {
                Id = 6,
                Name = "raiva",
                Keywords = "raiva,ira,irritado,furioso,bravo,indignado",
                Description = "Sentimento de raiva ou irritação",
                RecommendationType = "perdão"
            },
            new Emotion
            {
                Id = 7,
                Name = "gratidão",
                Keywords = "grato,agradecido,reconhecido,gratifico",
                Description = "Sentimento de gratidão e reconhecimento",
                RecommendationType = "ação de graças"
            },
            new Emotion
            {
                Id = 8,
                Name = "esperança",
                Keywords = "esperança,esperançoso,otimista,confiante",
                Description = "Sentimento de esperança e confiança no futuro",
                RecommendationType = "encorajamento"
            }
        );
    }
}

