using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace WebApplication3.Models;

public partial class WebdesignContext : DbContext
{
    public WebdesignContext()
    {
    }

    public WebdesignContext(DbContextOptions<WebdesignContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Dept> Depts { get; set; }

    public virtual DbSet<Dnotice> Dnotices { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Mpay> Mpays { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("persist security info=True;data source=202.200.144.153;port=3306;initial catalog=webdesign;user id=webdesign;password=123456;character set=utf8;allow zero datetime=true;convert zero datetime=true;pooling=true;maximumpoolsize=3000", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.2.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.Aid).HasName("PRIMARY");

            entity.ToTable("administrators");

            entity.Property(e => e.Aid)
                .ValueGeneratedNever()
                .HasColumnName("aid");
            entity.Property(e => e.Aname)
                .HasMaxLength(50)
                .HasColumnName("aname");
        });

        modelBuilder.Entity<Dept>(entity =>
        {
            entity.HasKey(e => e.Did).HasName("PRIMARY");

            entity
                .ToTable("dept")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.Did)
                .ValueGeneratedNever()
                .HasColumnName("did");
            entity.Property(e => e.Dname)
                .HasMaxLength(50)
                .HasColumnName("dname");
            entity.Property(e => e.Dtel)
                .HasMaxLength(50)
                .HasColumnName("dtel");
        });

        modelBuilder.Entity<Dnotice>(entity =>
        {
            entity.HasKey(e => e.Nuid).HasName("PRIMARY");

            entity
                .ToTable("dnotice")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.Npost, "Npost");

            entity.HasIndex(e => e.Nposto, "Nposto");

            entity.Property(e => e.Nuid)
                .ValueGeneratedNever()
                .HasColumnName("NUid");
            entity.Property(e => e.Naddtime).HasMaxLength(50);
            entity.Property(e => e.Ncontime).HasMaxLength(50);

            entity.HasOne(d => d.NpostNavigation).WithMany(p => p.DnoticeNpostNavigations)
                .HasForeignKey(d => d.Npost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dnotice_ibfk_2");

            entity.HasOne(d => d.NpostoNavigation).WithMany(p => p.DnoticeNpostoNavigations)
                .HasForeignKey(d => d.Nposto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dnotice_ibfk_3");

            entity.HasOne(d => d.Nu).WithOne(p => p.Dnotice)
                .HasForeignKey<Dnotice>(d => d.Nuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dnotice_ibfk_1");
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__efmigrationshistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Mpay>(entity =>
        {
            entity.HasKey(e => e.Mid).HasName("PRIMARY");

            entity
                .ToTable("mpays")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.Pid, "pid");

            entity.Property(e => e.Mid)
                .ValueGeneratedNever()
                .HasColumnName("mid");
            entity.Property(e => e.Mlevel)
                .HasMaxLength(50)
                .HasColumnName("mlevel");
            entity.Property(e => e.Mpay1).HasColumnName("mpay");
            entity.Property(e => e.Pid).HasColumnName("pid");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Mpays)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("mpays_ibfk_1");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PRIMARY");

            entity
                .ToTable("posts")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.Did, "did");

            entity.Property(e => e.Pid)
                .ValueGeneratedNever()
                .HasColumnName("pid");
            entity.Property(e => e.Did).HasColumnName("did");
            entity.Property(e => e.Pname)
                .HasMaxLength(50)
                .HasColumnName("pname");

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("posts_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("PRIMARY");

            entity
                .ToTable("user")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.Did, "did");

            entity.HasIndex(e => e.Mid, "mid");

            entity.HasIndex(e => e.Pid, "pid");

            entity.Property(e => e.Uid)
                .ValueGeneratedNever()
                .HasColumnName("uid");
            entity.Property(e => e.Did).HasColumnName("did");
            entity.Property(e => e.Mid).HasColumnName("mid");
            entity.Property(e => e.Pid).HasColumnName("pid");
            entity.Property(e => e.Uadress)
                .HasMaxLength(50)
                .HasColumnName("uadress");
            entity.Property(e => e.Umail)
                .HasMaxLength(50)
                .HasColumnName("umail");
            entity.Property(e => e.Uname)
                .HasMaxLength(50)
                .HasColumnName("uname");
            entity.Property(e => e.Urzsj)
                .HasMaxLength(50)
                .HasColumnName("urzsj");
            entity.Property(e => e.Usex)
                .HasMaxLength(50)
                .HasColumnName("usex");
            entity.Property(e => e.Usfzh)
                .HasMaxLength(50)
                .HasColumnName("usfzh");
            entity.Property(e => e.Ustatus)
                .HasMaxLength(50)
                .HasColumnName("ustatus");
            entity.Property(e => e.Utel)
                .HasMaxLength(50)
                .HasColumnName("utel");

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("user_ibfk_2");

            entity.HasOne(d => d.MidNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Mid)
                .HasConstraintName("user_ibfk_3");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("user_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
