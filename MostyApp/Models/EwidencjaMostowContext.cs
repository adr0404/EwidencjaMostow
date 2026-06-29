using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MostyApp.Models;

public partial class EwidencjaMostowContext : DbContext
{
    public EwidencjaMostowContext()
    {
    }

    public EwidencjaMostowContext(DbContextOptions<EwidencjaMostowContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Inspektorzy> Inspektorzy { get; set; }

    public virtual DbSet<Obiekty> Obiekty { get; set; }

    public virtual DbSet<Przeglady> Przeglady { get; set; }

    public virtual DbSet<SlkategorieUprawnien> SlkategorieUprawnien { get; set; }

    public virtual DbSet<SltypyKonstrukcji> SltypyKonstrukcji { get; set; }

    public virtual DbSet<SltypyPrzegladow> SltypyPrzegladow { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=EwidencjaMostow;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inspektorzy>(entity =>
        {
            entity.HasKey(e => e.InspektorId).HasName("PK__Inspekto__AC207F3BBBB468B7");

            entity.HasIndex(e => e.NrUprawnien, "UQ__Inspekto__62923FBB33AFD85D").IsUnique();

            entity.Property(e => e.InspektorId).HasColumnName("InspektorID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Imie)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.KategoriaUprawnienId).HasColumnName("KategoriaUprawnienID");
            entity.Property(e => e.Nazwisko)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NrUprawnien)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefon)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.KategoriaUprawnien).WithMany(p => p.Inspektorzy)
                .HasForeignKey(d => d.KategoriaUprawnienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inspektorzy_Kategoria");
        });

        modelBuilder.Entity<Obiekty>(entity =>
        {
            entity.HasKey(e => e.ObiektId).HasName("PK__Obiekty__9BB5189EF39941FD");

            entity.HasIndex(e => new { e.NazwaObiektu, e.OpisLokalizacji }, "UQ_Obiekt_NazwaLokalizacja").IsUnique();

            entity.Property(e => e.ObiektId).HasColumnName("ObiektID");
            entity.Property(e => e.NazwaObiektu)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.NosnoscEwidencyjna).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.OpisLokalizacji)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.StatusEksploatacyjny)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TypKonstrukcjiId).HasColumnName("TypKonstrukcjiID");

            entity.HasOne(d => d.TypKonstrukcji).WithMany(p => p.Obiekty)
                .HasForeignKey(d => d.TypKonstrukcjiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Obiekty_TypKonstrukcji");
        });

        modelBuilder.Entity<Przeglady>(entity =>
        {
            entity.HasKey(e => e.PrzegladId).HasName("PK__Przeglad__55C490D9DDCCFC9E");

            entity.Property(e => e.PrzegladId).HasColumnName("PrzegladID");
            entity.Property(e => e.DataWpisuDoSystemu)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InspektorId).HasColumnName("InspektorID");
            entity.Property(e => e.KosztRozliczeniowy).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ObiektId).HasColumnName("ObiektID");
            entity.Property(e => e.TypPrzegladuId).HasColumnName("TypPrzegladuID");
            entity.Property(e => e.Uwagi)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ZatwierdzonaNosnosc).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.Inspektor).WithMany(p => p.Przeglady)
                .HasForeignKey(d => d.InspektorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Przeglady_Inspektor");

            entity.HasOne(d => d.Obiekt).WithMany(p => p.Przeglady)
                .HasForeignKey(d => d.ObiektId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Przeglady_Obiekt");

            entity.HasOne(d => d.TypPrzegladu).WithMany(p => p.Przeglady)
                .HasForeignKey(d => d.TypPrzegladuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Przeglady_Typ");
        });

        modelBuilder.Entity<SlkategorieUprawnien>(entity =>
        {
            entity.HasKey(e => e.KategoriaUprawnienId).HasName("PK__SLKatego__8BEC511AA0CF19EE");

            entity.ToTable("SLKategorieUprawnien");

            entity.Property(e => e.KategoriaUprawnienId).HasColumnName("KategoriaUprawnienID");
            entity.Property(e => e.NazwaKategorii)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SltypyKonstrukcji>(entity =>
        {
            entity.HasKey(e => e.TypKonstrukcjiId).HasName("PK__SLTypyKo__B81886623047738C");

            entity.ToTable("SLTypyKonstrukcji");

            entity.Property(e => e.TypKonstrukcjiId).HasColumnName("TypKonstrukcjiID");
            entity.Property(e => e.NazwaTypu)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SltypyPrzegladow>(entity =>
        {
            entity.HasKey(e => e.TypPrzegladuId).HasName("PK__SLTypyPr__0A09ADEBBBE9E08D");

            entity.ToTable("SLTypyPrzegladow");

            entity.Property(e => e.TypPrzegladuId).HasColumnName("TypPrzegladuID");
            entity.Property(e => e.KosztEwidencyjny).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NazwaTypu)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.WaznoscWmiesiacach).HasColumnName("WaznoscWMiesiacach");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
