using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RPG_Sheet_characters_admin.Models;

public partial class ScareContext : DbContext
{
    public ScareContext()
    {
    }

    public ScareContext(DbContextOptions<ScareContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UsuarioModel> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<UsuarioModel>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");

            entity.Property(e => e.Senha)
                .HasMaxLength(255)
                .HasColumnName("senha");

            entity.Property(e => e.Tipo)
                .HasMaxLength(10)
                .HasColumnName("tipo");

            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .HasColumnName("usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}