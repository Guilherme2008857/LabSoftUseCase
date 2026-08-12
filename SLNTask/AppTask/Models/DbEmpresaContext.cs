using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppTask.Models;

public partial class DbEmpresaContext : DbContext
{
    public DbEmpresaContext()
    {
    }

    public DbEmpresaContext(DbContextOptions<DbEmpresaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Incidente> Incidentes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConexaoSqlServer");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Incidente>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Incident__06370DAD28B51527");

            entity.ToTable("Incidente");

            entity.Property(e => e.DataIncidente).HasColumnType("datetime");
            entity.Property(e => e.DescricaoProblema)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Resolvido)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Solucao)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
