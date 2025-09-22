using FizzWare.NBuilder;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using TesteFacil.Testes.Integracao.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;

namespace GestaoDeEstacionamento.Testes.Integracao.ModuloTicket;

[TestClass]
[TestCategory("Testes de Integração de Ticket")]
public sealed class RepositorioTicketEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Ticket_CorretamenteAsync()
    {
        // Criar Hospede
        var hospede = new Hospede("Iago", "123.123.123-12");
        await dbContext.Hospedes.AddAsync(hospede);
        await dbContext.SaveChangesAsync();

        // 2Criar Veiculo
        var veiculo = new Veiculo("ABC-1234", "Gol", "Preto", hospede, null);
        await dbContext.Veiculos.AddAsync(veiculo);
        await dbContext.SaveChangesAsync();

        // Criar Faturamento
        var faturamento = new Faturamento
        {
            ValorTotal = 100,
            DataPagamento = DateTime.UtcNow
        };
        await dbContext.Faturamentos.AddAsync(faturamento);
        await dbContext.SaveChangesAsync(); // gera Id

        // Criar Ticket associando Veiculo e Faturamento
        var ticket = new Ticket
        {
            Veiculo = veiculo,
            FaturamentoId = faturamento.Id,
            DataEntrada = DateTime.UtcNow
        };
        await repositorioTicket!.CadastrarAsync(ticket);
        await dbContext.SaveChangesAsync(); // salva Ticket

        // Criar Vaga associada ao Ticket
        var vaga = new Vaga(1) { TicketId = ticket.Id };
        await dbContext.Vagas.AddAsync(vaga);
        await dbContext.SaveChangesAsync();

        ticket.Vaga = vaga;
        await dbContext.SaveChangesAsync();

        // Assert
        var ticketSelecionado = await repositorioTicket.SelecionarRegistroPorIdAsync(ticket.Id);
        Assert.AreEqual(ticket.Id, ticketSelecionado!.Id);
    }


    [TestMethod]
    public async Task Deve_Editar_Ticket_CorretamenteAsync()
    {
        // 1Criar Hospede
        var hospede = new Hospede("Iago", "123.123.123-12");
        await dbContext.Hospedes.AddAsync(hospede);
        await dbContext.SaveChangesAsync();

        // Criar Veiculo
        var veiculo = new Veiculo("ABC-1234", "Gol", "Preto", hospede, null);
        await dbContext.Veiculos.AddAsync(veiculo);
        await dbContext.SaveChangesAsync();

        // 3Criar Faturamento
        var faturamento = new Faturamento
        {
            ValorTotal = 100,
            DataPagamento = DateTime.UtcNow
        };
        await dbContext.Faturamentos.AddAsync(faturamento);
        await dbContext.SaveChangesAsync();

        // Criar Ticket
        var ticket = new Ticket
        {
            Veiculo = veiculo,
            FaturamentoId = faturamento.Id,
            DataEntrada = DateTime.UtcNow
        };
        await repositorioTicket!.CadastrarAsync(ticket);
        await dbContext.SaveChangesAsync();

        // Criar Vaga vinculada ao Ticket
        var vaga = new Vaga(1) { TicketId = ticket.Id };
        await dbContext.Vagas.AddAsync(vaga);
        await dbContext.SaveChangesAsync();

        ticket.Vaga = vaga;
        await dbContext.SaveChangesAsync();

        // 6Editar o Ticket (exemplo: mudar DataSaida + atualizar número da vaga)
        ticket.DataSaida = DateTime.UtcNow.AddHours(2);
        ticket.Vaga.Numero = 2; // 🔑 altera a vaga existente (não cria outra!)

        var conseguiuEditar = await repositorioTicket.EditarAsync(ticket.Id, ticket);
        await dbContext.SaveChangesAsync();

        // Assert
        var ticketSelecionado = await repositorioTicket.SelecionarRegistroPorIdAsync(ticket.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(ticketSelecionado);
        Assert.AreEqual(ticket.Id, ticketSelecionado!.Id);
        Assert.AreEqual(2, ticketSelecionado.Vaga!.Numero); // confirma mudança na vaga
        Assert.IsNotNull(ticketSelecionado.DataSaida);      // confirma saída alterada
    }

    [TestMethod]
    public async Task Deve_Excluir_Ticket_CorretamenteAsync()
    {
        // Criar Hospede
        var hospede = new Hospede("Iago", "123.123.123-12");
        await dbContext.Hospedes.AddAsync(hospede);
        await dbContext.SaveChangesAsync();

        // Criar Veiculo
        var veiculo = new Veiculo("ABC-1234", "Gol", "Preto", hospede, null);
        await dbContext.Veiculos.AddAsync(veiculo);
        await dbContext.SaveChangesAsync();

        // Criar Faturamento
        var faturamento = new Faturamento { ValorTotal = 100, DataPagamento = DateTime.UtcNow };
        await dbContext.Faturamentos.AddAsync(faturamento);
        await dbContext.SaveChangesAsync();

        // Criar Ticket
        var ticket = new Ticket { Veiculo = veiculo, FaturamentoId = faturamento.Id, DataEntrada = DateTime.UtcNow };
        await repositorioTicket.CadastrarAsync(ticket);
        await dbContext.SaveChangesAsync();

        // Criar Vaga
        var vaga = new Vaga(1) { TicketId = ticket.Id };
        await dbContext.Vagas.AddAsync(vaga);
        await dbContext.SaveChangesAsync();

        ticket.Vaga = vaga;
        await dbContext.SaveChangesAsync();

        // Dissociar vaga antes de excluir para não quebrar FK
        dbContext.Vagas.Remove(vaga);
        await dbContext.SaveChangesAsync();

        // Act
        var conseguiuExcluir = await repositorioTicket.ExcluirAsync(ticket.Id);
        await dbContext.SaveChangesAsync();

        // Assert
        var registroSelecionado = await repositorioTicket.SelecionarRegistroPorIdAsync(ticket.Id);
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Tickets_CorretamenteAsync()
    {
        // Arrange
        // Criar hospedes
        var hospede1 = new Hospede("Iago", "123.123.123-12");
        var hospede2 = new Hospede("Maria", "321.321.321-32");
        var hospede3 = new Hospede("João", "456.456.456-45");

        await dbContext.Hospedes.AddRangeAsync(hospede1, hospede2, hospede3);
        await dbContext.SaveChangesAsync();

        // Criar veiculos
        var veiculo1 = new Veiculo("ABC-1234", "Ford", "Branco", hospede1, null);
        var veiculo2 = new Veiculo("BCD-1234", "Fiat", "Preto", hospede2, null);
        var veiculo3 = new Veiculo("CDE-1234", "Chevrolet", "Vermelho", hospede3, null);

        await dbContext.Veiculos.AddRangeAsync(veiculo1, veiculo2, veiculo3);
        await dbContext.SaveChangesAsync();

        // Criar faturamentos
        var faturamento1 = new Faturamento(DateTime.UtcNow, 100, null);
        var faturamento2 = new Faturamento(DateTime.UtcNow, 101, null);
        var faturamento3 = new Faturamento(DateTime.UtcNow, 102, null);

        await dbContext.Faturamentos.AddRangeAsync(faturamento1, faturamento2, faturamento3);
        await dbContext.SaveChangesAsync();

        // Criar tickets manualmente
        var ticket1 = new Ticket(veiculo1, null)
        {
            FaturamentoId = faturamento1.Id,
            DataEntrada = DateTime.UtcNow
        };
        var ticket2 = new Ticket(veiculo2, null)
        {
            FaturamentoId = faturamento2.Id,
            DataEntrada = DateTime.UtcNow
        };
        var ticket3 = new Ticket(veiculo3, null)
        {
            FaturamentoId = faturamento3.Id,
            DataEntrada = DateTime.UtcNow
        };

        await dbContext.Tickets.AddRangeAsync(ticket1, ticket2, ticket3);
        await dbContext.SaveChangesAsync();

        // Criar vagas manualmente
        var vaga1 = new Vaga(1) { TicketId = ticket1.Id };
        var vaga2 = new Vaga(2) { TicketId = ticket2.Id };
        var vaga3 = new Vaga(3) { TicketId = ticket3.Id };

        await dbContext.Vagas.AddRangeAsync(vaga1, vaga2, vaga3);
        await dbContext.SaveChangesAsync();

        // Limpar tracking
        dbContext.ChangeTracker.Clear();

        // Act
        var ticketsRecebidos = (await repositorioTicket.SelecionarRegistrosAsync())
            .OrderBy(t => t.Id)
            .ToList();

        // Assert
        Assert.AreEqual(3, ticketsRecebidos.Count);

        var idsEsperados = new List<Guid> { ticket1.Id, ticket2.Id, ticket3.Id }.OrderBy(id => id).ToList();
        var idsRecebidos = ticketsRecebidos.Select(t => t.Id).OrderBy(id => id).ToList();
        CollectionAssert.AreEqual(idsEsperados, idsRecebidos);
    }
}