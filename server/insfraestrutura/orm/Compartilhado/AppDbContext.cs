using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

public class AppDbContext(DbContextOptions options, ITenantProvider? tenantProvider = null) :
    IdentityDbContext<Usuario, Cargo, Guid>(options), IUnitOfWork
{
    public DbSet<Vaga> Vagas { get; set; }
    public DbSet<Hospede> Hospedes { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Faturamento> Faturamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (tenantProvider is not null)
        {
            modelBuilder.Entity<Vaga>()
                .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

            modelBuilder.Entity<Hospede>()
                .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

            modelBuilder.Entity<Veiculo>()
                .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

            modelBuilder.Entity<Ticket>()
                .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

            modelBuilder.Entity<Faturamento>()
                .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));
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
