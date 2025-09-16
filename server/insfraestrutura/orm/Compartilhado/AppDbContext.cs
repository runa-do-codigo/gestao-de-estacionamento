﻿using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

public class AppDbContext(DbContextOptions options, ITenantProvider? tenantProvider = null) :
    IdentityDbContext<Usuario, Cargo, Guid>(options), IUnitOfWork
{
    //public DbSet<Contato> Contatos { get; set; }
    public DbSet<Faturamento> Faturamentos { get; set; }
    //public DbSet<Categoria> Categorias { get; set; }
    //public DbSet<Despesa> Despesas { get; set; }
    //public DbSet<Tarefa> Tarefas { get; set; }
    //public DbSet<ItemTarefa> ItensTarefa { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (tenantProvider is not null)
        {
            //modelBuilder.Entity<Contato>()
            //    .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

            modelBuilder.Entity<Faturamento>()
                .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

            //modelBuilder.Entity<Categoria>()
            //    .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

            //modelBuilder.Entity<Despesa>()
            //    .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

            //modelBuilder.Entity<Tarefa>()
            //    .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

            //modelBuilder.Entity<ItemTarefa>()
            //    .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));
        }

        var assembly = typeof(AppDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }

    public async Task CommitAsync()
    {
        await SaveChangesAsync();
    }

    public async Task RollbackAsync()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Unchanged;
                    break;

                case EntityState.Modified:
                    entry.State = EntityState.Unchanged;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }

        await Task.CompletedTask;
    }
}
