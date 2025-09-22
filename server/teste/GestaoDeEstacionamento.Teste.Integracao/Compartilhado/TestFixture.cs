using DotNet.Testcontainers.Containers;
using FizzWare.NBuilder;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFaturamento;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloHospede;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloTicket;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVaga;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVeiculo;
using Testcontainers.PostgreSql;

namespace TesteFacil.Testes.Integracao.Compartilhado;

[TestClass]
public abstract class TestFixture
{
    protected AppDbContext? dbContext;
    protected RepositorioVagaEmOrm? repositorioVaga;
    protected RepositorioHospedeEmOrm? repositorioHospede;
    protected RepositorioVeiculoEmOrm? repositorioVeiculo;
    protected RepositorioTicketEmOrm? repositorioTicket;
    protected RepositorioFaturamentoEmOrm? repositorioFaturamento;

    private static IDatabaseContainer? container;

    [AssemblyInitialize]
    public static async Task Setup(TestContext _)
    {
        container = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithName("gestao-de-estacionamento-testdb")
            .WithDatabase("GestaoDeEstacionamentoTestDb")
            .WithUsername("postgres")
            .WithPassword("YourStrongPassword")
            .WithCleanUp(true)
            .Build();

        await InicializarBancoDadosAsync(container);
    }

    [AssemblyCleanup]
    public static async Task Teardown()
    {
        await EncerrarBancoDadosAsync();
    }

    [TestInitialize]
    public void ConfigurarTestes()
    {
        if (container is null)
            throw new ArgumentNullException("O banco de dados não foi inicializado.");

        dbContext = AppDbContextFactory.CriarDbContext(container.GetConnectionString());

        ConfigurarTabelas(dbContext);

        repositorioVaga = new RepositorioVagaEmOrm(dbContext);
        repositorioHospede = new RepositorioHospedeEmOrm(dbContext);
        repositorioVeiculo = new RepositorioVeiculoEmOrm(dbContext);
        repositorioTicket = new RepositorioTicketEmOrm(dbContext);
        repositorioFaturamento = new RepositorioFaturamentoEmOrm(dbContext);

        // adaptando para executar o async de forma síncrona
        BuilderSetup.SetCreatePersistenceMethod<Vaga>(h => repositorioVaga.CadastrarAsync(h).GetAwaiter().GetResult());
        BuilderSetup.SetCreatePersistenceMethod<IList<Vaga>>(h => repositorioVaga.CadastrarEntidades(h).GetAwaiter().GetResult());

        BuilderSetup.SetCreatePersistenceMethod<Hospede>(h => repositorioHospede.CadastrarAsync(h).GetAwaiter().GetResult());
        BuilderSetup.SetCreatePersistenceMethod<IList<Hospede>>(h => repositorioHospede.CadastrarEntidades(h).GetAwaiter().GetResult());

        BuilderSetup.SetCreatePersistenceMethod<Veiculo>(h => repositorioVeiculo.CadastrarAsync(h).GetAwaiter().GetResult());
        BuilderSetup.SetCreatePersistenceMethod<IList<Veiculo>>(h => repositorioVeiculo.CadastrarEntidades(h).GetAwaiter().GetResult());

        BuilderSetup.SetCreatePersistenceMethod<Ticket>(h => repositorioTicket.CadastrarAsync(h).GetAwaiter().GetResult());
        BuilderSetup.SetCreatePersistenceMethod<IList<Ticket>>(h => repositorioTicket.CadastrarEntidades(h).GetAwaiter().GetResult());

        BuilderSetup.SetCreatePersistenceMethod<Faturamento>(h => repositorioFaturamento.CadastrarAsync(h).GetAwaiter().GetResult());
        BuilderSetup.SetCreatePersistenceMethod<IList<Faturamento>>(h => repositorioFaturamento.CadastrarEntidades(h).GetAwaiter().GetResult());
    }

    private static void ConfigurarTabelas(AppDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        dbContext.Faturamentos.RemoveRange(dbContext.Faturamentos);
        dbContext.Tickets.RemoveRange(dbContext.Tickets);
        dbContext.Veiculos.RemoveRange(dbContext.Veiculos);
        dbContext.Hospedes.RemoveRange(dbContext.Hospedes);
        dbContext.Vagas.RemoveRange(dbContext.Vagas);

        dbContext.SaveChanges();
    }

    private static async Task InicializarBancoDadosAsync(IDatabaseContainer container)
    {
        await container.StartAsync();
    }

    private static async Task EncerrarBancoDadosAsync()
    {
        if (container is null)
            throw new ArgumentNullException("O Banco de dados não foi inicializado.");

        await container.StopAsync();
        await container.DisposeAsync();
    }
}