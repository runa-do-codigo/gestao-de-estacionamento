using FizzWare.NBuilder;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using TesteFacil.Testes.Integracao.Compartilhado;

namespace GestaoDeEstacionamento.Testes.Integracao.ModuloFaturamento;

[TestClass]
[TestCategory("Testes de Integração de Faturamento")]
public sealed class RepositorioFaturamentoEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Faturamento_CorretamenteAsync()
    {
        // Criar Hospede
        var hospede = new Hospede("Iago", "123.123.123-12");
        await dbContext.Hospedes.AddAsync(hospede);
        await dbContext.SaveChangesAsync();

        // Criar Veiculo
        var veiculo = new Veiculo("ABC-1234", "Gol", "Preto", hospede, null);
        await dbContext.Veiculos.AddAsync(veiculo);
        await dbContext.SaveChangesAsync();

        // Criar Faturamento primeiro (sem Ticket associado)
        var faturamento = new Faturamento(DateTime.UtcNow, 20, null);
        await repositorioFaturamento!.CadastrarAsync(faturamento);
        await dbContext.SaveChangesAsync();

        // Criar Ticket associando Faturamento existente
        var ticket = new Ticket
        {
            Veiculo = veiculo,
            FaturamentoId = faturamento.Id,
            DataEntrada = DateTime.UtcNow
        };
        await dbContext.Tickets.AddAsync(ticket);
        await dbContext.SaveChangesAsync();

        // Atualizar Faturamento para vincular o Ticket (se necessário)
        faturamento.Ticket = ticket;
        await dbContext.SaveChangesAsync();

        // Assert
        var faturamentoSelecionado = await repositorioFaturamento.SelecionarRegistroPorIdAsync(faturamento.Id);
        Assert.IsNotNull(faturamentoSelecionado);
        Assert.AreEqual(20, faturamentoSelecionado!.ValorTotal);
        Assert.AreEqual(ticket.Id, faturamentoSelecionado.Ticket.Id);
    }

    [TestMethod]
    public async Task Deve_Editar_Faturamento_CorretamenteAsync()
    {
        // Criar Hospede
        var hospede = new Hospede("João", "111.111.111-11");
        await dbContext.Hospedes.AddAsync(hospede);
        await dbContext.SaveChangesAsync();

        // Criar Veiculo
        var veiculo = new Veiculo("XYZ-9876", "Palio", "Branco", hospede, null);
        await dbContext.Veiculos.AddAsync(veiculo);
        await dbContext.SaveChangesAsync();

        // Criar Faturamento primeiro (sem Ticket)
        var faturamento = new Faturamento(DateTime.UtcNow, 20, null);
        await repositorioFaturamento!.CadastrarAsync(faturamento);
        await dbContext.SaveChangesAsync();

        // Criar Vaga e Ticket associando o Faturamento existente
        var vaga = new Vaga(2);
        await dbContext.Vagas.AddAsync(vaga);

        var ticket = new Ticket
        {
            Veiculo = veiculo,
            FaturamentoId = faturamento.Id,
            Vaga = vaga,
            DataEntrada = DateTime.UtcNow
        };
        await dbContext.Tickets.AddAsync(ticket);
        await dbContext.SaveChangesAsync();

        // Atualiza Faturamento para vincular o Ticket
        faturamento.Ticket = ticket;
        await dbContext.SaveChangesAsync();

        // Editar Faturamento
        var faturamentoEditado = new Faturamento(DateTime.UtcNow, 30, ticket);
        var conseguiuEditar = await repositorioFaturamento.EditarAsync(faturamento.Id, faturamentoEditado);
        await dbContext.SaveChangesAsync();

        // Assert
        var registroSelecionado = await repositorioFaturamento.SelecionarRegistroPorIdAsync(faturamento.Id);
        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(30, registroSelecionado!.ValorTotal);
    }

    [TestMethod]
    public async Task Deve_Excluir_Faturamento_CorretamenteAsync()
    {
        // Criar Hospede
        var hospede = new Hospede("Maria", "222.222.222-22");
        await dbContext.Hospedes.AddAsync(hospede);
        await dbContext.SaveChangesAsync();

        // Criar Veiculo
        var veiculo = new Veiculo("LMN-5555", "Corsa", "Prata", hospede, null);
        await dbContext.Veiculos.AddAsync(veiculo);
        await dbContext.SaveChangesAsync();

        // Criar Faturamento primeiro
        var faturamento = new Faturamento(DateTime.UtcNow, 20, null);
        await repositorioFaturamento!.CadastrarAsync(faturamento);
        await dbContext.SaveChangesAsync();

        // Criar Vaga e Ticket associando o Faturamento
        var vaga = new Vaga(3);
        await dbContext.Vagas.AddAsync(vaga);

        var ticket = new Ticket
        {
            Veiculo = veiculo,
            FaturamentoId = faturamento.Id,
            Vaga = vaga,
            DataEntrada = DateTime.UtcNow
        };
        await dbContext.Tickets.AddAsync(ticket);
        await dbContext.SaveChangesAsync();

        // Atualiza Faturamento para vincular o Ticket
        faturamento.Ticket = ticket;
        await dbContext.SaveChangesAsync();

        // Excluir Faturamento
        var conseguiuExcluir = await repositorioFaturamento.ExcluirAsync(faturamento.Id);
        await dbContext.SaveChangesAsync();

        // Assert
        var registroSelecionado = await repositorioFaturamento.SelecionarRegistroPorIdAsync(faturamento.Id);
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Todos_Faturamentos_CorretamenteAsync()
    {
        // Arrange
        var hospede = new Hospede("Iago", "123.123.123-12");
        var veiculo = new Veiculo("ABC-1234", "Gol", "Preto", hospede, null);
        await dbContext.Hospedes.AddAsync(hospede);
        await dbContext.Veiculos.AddAsync(veiculo);
        await dbContext.SaveChangesAsync();

        // Criar Faturamentos manualmente
        var faturamento1 = new Faturamento(DateTime.UtcNow, 10, null);
        var faturamento2 = new Faturamento(DateTime.UtcNow, 20, null);
        var faturamento3 = new Faturamento(DateTime.UtcNow, 30, null);
        await dbContext.Faturamentos.AddRangeAsync(faturamento1, faturamento2, faturamento3);
        await dbContext.SaveChangesAsync();

        // Criar Tickets associados aos Faturamentos
        var ticket1 = new Ticket(veiculo, null) { FaturamentoId = faturamento1.Id, DataEntrada = DateTime.UtcNow };
        var ticket2 = new Ticket(veiculo, null) { FaturamentoId = faturamento2.Id, DataEntrada = DateTime.UtcNow };
        var ticket3 = new Ticket(veiculo, null) { FaturamentoId = faturamento3.Id, DataEntrada = DateTime.UtcNow };
        await dbContext.Tickets.AddRangeAsync(ticket1, ticket2, ticket3);
        await dbContext.SaveChangesAsync();

        // Limpar tracking para evitar conflitos
        dbContext.ChangeTracker.Clear();

        // Act
        var faturamentosRecebidos = await repositorioFaturamento.SelecionarRegistrosAsync();

        // Assert
        Assert.AreEqual(3, faturamentosRecebidos.Count);

        var idsEsperados = new List<Guid> { faturamento1.Id, faturamento2.Id, faturamento3.Id };
        var idsRecebidos = faturamentosRecebidos.Select(f => f.Id).ToList();
        CollectionAssert.AreEquivalent(idsEsperados, idsRecebidos);
    }

    [TestMethod]
    public async Task Deve_Obter_Valor_Faturamentos_CorretamenteAsync()
    {
        // Criar Hospede
        var hospede = new Hospede("Lucas", "444.444.444-44");
        await dbContext!.Hospedes.AddAsync(hospede);
        await dbContext.SaveChangesAsync();

        // Criar Veiculo
        var veiculo = new Veiculo("DDD-4444", "HB20", "Azul", hospede, null);
        await dbContext.Veiculos.AddAsync(veiculo);
        await dbContext.SaveChangesAsync();

        // Criar Faturamento primeiro (sem Ticket)
        var faturamento = new Faturamento(DateTime.UtcNow, null, null);
        await repositorioFaturamento!.CadastrarAsync(faturamento);
        await dbContext.SaveChangesAsync();

        // Criar Vaga e Ticket
        var vaga = new Vaga(7);
        await dbContext.Vagas.AddAsync(vaga);

        var ticket = new Ticket
        {
            Veiculo = veiculo,
            Vaga = vaga,
            FaturamentoId = faturamento.Id,
            DataEntrada = DateTime.UtcNow
        };

        ticket.DataSaida = ticket.DataEntrada.AddHours(1); // simula saída 1h depois
        await dbContext.Tickets.AddAsync(ticket);
        await dbContext.SaveChangesAsync();

        // Vincular Ticket ao Faturamento
        faturamento.Ticket = ticket;
        await dbContext.SaveChangesAsync();

        // Act
        var valor = await repositorioFaturamento.ObterTotalFatura(faturamento.Id);

        // Assert
        Assert.AreEqual(10, valor); // int
    }
}