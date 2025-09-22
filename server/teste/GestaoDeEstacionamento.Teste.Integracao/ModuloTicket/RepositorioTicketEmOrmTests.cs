using FizzWare.NBuilder;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using TesteFacil.Testes.Integracao.Compartilhado;

namespace GestaoDeEstacionamento.Testes.Integracao.ModuloTicket;

[TestClass]
[TestCategory("Testes de Integração de Ticket")]
public sealed class RepositorioTicketEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Ticket_CorretamenteAsync()
    {
        // Arrange
        var veiculo = Builder<Veiculo>.CreateNew().Persist();
        var vaga = Builder<Vaga>.CreateNew().Persist();

        var ticket = new Ticket(veiculo, vaga);

        // Act
        repositorioTicket?.CadastrarAsync(ticket);
        dbContext?.SaveChanges();

        // Assert
        var ticketSelecionada = await repositorioTicket?.SelecionarRegistroPorIdAsync(ticket.Id);

        Assert.AreEqual(ticket, ticketSelecionada);
    }

    [TestMethod]
    public async Task Deve_Editar_Ticket_CorretamenteAsync()
    {
        // Arrange
        var veiculo = Builder<Veiculo>.CreateNew().Persist();
        var vaga = Builder<Vaga>.CreateNew().Persist();

        var ticket = new Ticket(veiculo, vaga);
        repositorioTicket?.CadastrarAsync(ticket);
        dbContext?.SaveChanges();

        var vaga2 = Builder<Vaga>.CreateNew().Persist();
        var ticketEditado = new Ticket(veiculo, vaga2);

        // Act
        var conseguiuEditar = await repositorioTicket?.EditarAsync(ticket.Id, ticketEditado);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioTicket?.SelecionarRegistroPorIdAsync(ticket.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(ticket, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Excluir_Ticket_CorretamenteAsync()
    {
        // Arrange
        var veiculo = Builder<Veiculo>.CreateNew().Persist();
        var vaga = Builder<Vaga>.CreateNew().Persist();

        var ticket = new Ticket(veiculo, vaga);
        repositorioTicket?.CadastrarAsync(ticket);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = await repositorioTicket?.ExcluirAsync(ticket.Id);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioTicket?.SelecionarRegistroPorIdAsync(ticket.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Tickets_CorretamenteAsync()
    {
        // Arrange - Arranjo
        var veiculo = Builder<Veiculo>.CreateListOfSize(3).Persist().ToList();
        var vaga = Builder<Vaga>.CreateListOfSize(3).Persist().ToList();

        var ticket = new Ticket(veiculo[0], vaga[0]);
        var ticket2 = new Ticket(veiculo[1], vaga[1]);
        var ticket3 = new Ticket(veiculo[2], vaga[2]);

        List<Ticket> ticketsEsperadas = [ticket, ticket2, ticket3];

        repositorioTicket?.CadastrarEntidades(ticketsEsperadas);
        dbContext?.SaveChanges();

        var ticketsEsperadasOrdenadas = ticketsEsperadas
            .OrderBy(d => d.Id)
            .ToList();

        // Act - Ação
        var ticketsRecebidas = await repositorioTicket?.SelecionarRegistrosAsync();

        // Assert - Asseção
        CollectionAssert.AreEqual(ticketsEsperadasOrdenadas, ticketsRecebidas);
    }
}