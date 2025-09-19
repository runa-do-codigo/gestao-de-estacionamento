using DotNet.Testcontainers.Containers;
using FizzWare.NBuilder;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Testcontainers.PostgreSql;

namespace TesteFacil.Testes.Integracao.Compartilhado;

[TestClass]
public abstract class TestFixture
{
    protected AppDbContext? dbContext;

    //protected RepositorioHospedeEmOrm? repositorioHospede;
    //protected RepositorioVeiculoEmOrm? repositorioVeiculo;
    //protected RepositorioTicketEmOrm? repositorioTicket;
    //protected RepositorioFaturamentoEmOrm? repositorioFaturamento;

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

        //repositorioHospede = new RepositorioHospedeEmOrm(dbContext);
        //repositorioVeiculo = new RepositorioVeiculoEmOrm(dbContext);
        //repositorioTicket = new RepositorioTicketEmOrm(dbContext);
        //repositorioFaturamento = new RepositorioFaturamentoEmOrm(dbContext);

    //    BuilderSetup.SetCreatePersistenceMethod<Hospede>(repositorioHospede.Cadastrar);
    //    BuilderSetup.SetCreatePersistenceMethod<IList<Hospede>>(repositorioHospede.CadastrarEntidades);

    //    BuilderSetup.SetCreatePersistenceMethod<Veiculo>(repositorioVeiculo.Cadastrar);
    //    BuilderSetup.SetCreatePersistenceMethod<IList<Veiculo>>(repositorioVeiculo.CadastrarEntidades);

    //    BuilderSetup.SetCreatePersistenceMethod<Ticket>(repositorioTicket.Cadastrar);
    //    BuilderSetup.SetCreatePersistenceMethod<IList<Ticket>>(repositorioTicket.CadastrarEntidades);

    //    BuilderSetup.SetCreatePersistenceMethod<Faturamento>(repositorioFaturamento.Cadastrar);
    //    BuilderSetup.SetCreatePersistenceMethod<IList<Faturamento>>(repositorioFaturamento.CadastrarEntidades);
    }

    private static void ConfigurarTabelas(AppDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        dbContext.Faturamentos.RemoveRange(dbContext.Faturamentos);
        dbContext.Tickets.RemoveRange(dbContext.Tickets);
        dbContext.Veiculos.RemoveRange(dbContext.Veiculos);
        dbContext.Hospedes.RemoveRange(dbContext.Hospedes);

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
