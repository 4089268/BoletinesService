﻿using System;
using System.Collections.Generic;
using BoletinesService.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoletinesService.Data;

public partial class SicemContext : DbContext
{
    public SicemContext()
    {
    }

    public SicemContext(DbContextOptions<SicemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BoletinMensaje> BoletinMensajes { get; set; }

    public virtual DbSet<Destinatario> Destinatarios { get; set; }

    public virtual DbSet<OprBoletin> OprBoletins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=SICEM");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoletinMensaje>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BoletinM__3213E83FE25E88AD");

            entity.ToTable("BoletinMensaje", "Boletin");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.BoletinId).HasColumnName("boletinId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("createdAt");
            entity.Property(e => e.DeletedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("deletedAt");
            entity.Property(e => e.EsArchivo)
                .HasDefaultValue(false)
                .HasColumnName("esArchivo");
            entity.Property(e => e.FileName)
                .IsUnicode(false)
                .HasColumnName("fileName");
            entity.Property(e => e.FileSize).HasColumnName("fileSize");
            entity.Property(e => e.Mensaje)
                .IsUnicode(false)
                .HasColumnName("mensaje");
            entity.Property(e => e.MimmeType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("mimmeType");

            entity.HasOne(d => d.Boletin).WithMany(p => p.BoletinMensajes)
                .HasForeignKey(d => d.BoletinId)
                .HasConstraintName("FK_BoletinMensaje_Boletin");
        });

        modelBuilder.Entity<Destinatario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Destinat__3213E83F405D3C05");

            entity.ToTable("Destinatario", "Boletin");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.BoletinId).HasColumnName("boletinId");
            entity.Property(e => e.EnvioMetadata)
                .IsUnicode(false)
                .HasColumnName("envioMetadata");
            entity.Property(e => e.Error).HasColumnName("error");
            entity.Property(e => e.FechaEnvio).HasColumnName("fechaEnvio");
            entity.Property(e => e.Lada)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("lada");
            entity.Property(e => e.Resultado)
                .IsUnicode(false)
                .HasColumnName("resultado");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.Boletin).WithMany(p => p.Destinatarios)
                .HasForeignKey(d => d.BoletinId)
                .HasConstraintName("FK_Destinatario_Boletin");
        });

        modelBuilder.Entity<OprBoletin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OprBolet__3213E83F504B1C73");

            entity.ToTable("OprBoletin", "Boletin");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("createdAt");
            entity.Property(e => e.FinishedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("finishedAt");
            entity.Property(e => e.Titulo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("titulo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
