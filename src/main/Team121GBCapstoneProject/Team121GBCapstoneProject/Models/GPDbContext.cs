using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

public partial class GPDbContext : DbContext
{
    public GPDbContext()
    {
    }

    public GPDbContext(DbContextOptions<GPDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GamePlayListType> GamePlayListTypes { get; set; }

    public virtual DbSet<ListName> ListNames { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonGameList> PersonGameLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=GPConnection");
        }
        optionsBuilder.UseLazyLoadingProxies();

    }
    // => optionsBuilder.UseSqlServer("Name=GPConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Game__3214EC27A6BC5A41");
        });

        modelBuilder.Entity<GamePlayListType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GamePlay__3214EC27154C6ACE");
        });

        modelBuilder.Entity<ListName>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ListName__3214EC27EF67E615");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Person__3214EC27DBF4ED51");
        });

        modelBuilder.Entity<PersonGameList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PersonGa__3214EC2778C1E2EC");

            entity.HasOne(d => d.Game).WithMany(p => p.PersonGameLists).HasConstraintName("FK_GameID");

            entity.HasOne(d => d.ListKind).WithMany(p => p.PersonGameLists).HasConstraintName("FK_ListKindID");

            entity.HasOne(d => d.ListName).WithMany(p => p.PersonGameLists).HasConstraintName("FK_ListNameID");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonGameLists).HasConstraintName("FK_PersonID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
