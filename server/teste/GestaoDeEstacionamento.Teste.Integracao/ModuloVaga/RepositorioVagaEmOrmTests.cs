using FizzWare.NBuilder;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using TesteFacil.Testes.Integracao.Compartilhado;

namespace GestaoDeEstacionamento.Testes.Integracao.ModuloVaga;

[TestClass]
[TestCategory("Testes de Integração da Vaga")]
public sealed class RepositorioVagaEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Vaga_CorretamenteAsync()
    {
        // Criar Hospede
        var hospede = new Hospede
        {
            Nome = "Iago",
            CPF = "123.123.123-12"
        };
        await dbContext.Hospedes.AddAsync(hospede);
        await dbContext.SaveChangesAsync();

        // Criar Veiculo associado ao Hospede
        var veiculo = Builder<Veiculo>.CreateNew()
            .With(v => v.Hospede, hospede)
            .Build();
        await dbContext.Veiculos.AddAsync(veiculo);
        await dbContext.SaveChangesAsync();

        // Atualizar Hospede com VeiculoId se necessário
        hospede.VeiculoId = veiculo.Id;
        await dbContext.SaveChangesAsync();

        // Criar Faturamento
        var faturamento = new Faturamento
        {
            ValorTotal = 100, // preencha com valores válidos
            DataPagamento = DateTime.UtcNow
        };
        await dbContext.Faturamentos.AddAsync(faturamento);
        await dbContext.SaveChangesAsync();

        // 5Criar Ticket associado ao Veiculo e Faturamento
        var ticket = new Ticket
        {
            DataEntrada = DateTime.UtcNow,
            Veiculo = veiculo,
            FaturamentoId = faturamento.Id
        };
        await dbContext.Tickets.AddAsync(ticket);
        await dbContext.SaveChangesAsync();

        // Criar Vaga associada ao Ticket
        var vaga = new Vaga(1)
        {
            TicketId = ticket.Id
        };
        await repositorioVaga!.CadastrarAsync(vaga);
        await dbContext!.SaveChangesAsync();

        // Assert
        var registroSelecionado = await repositorioVaga.SelecionarRegistroPorIdAsync(vaga.Id);
        Assert.AreEqual(vaga.Id, registroSelecionado!.Id);
    }

    [TestMethod]
    public async Task Deve_Editar_Vaga_CorretamenteAsync()
    {
        // Arrange - criar toda a cadeia de entidades
        var hospede = new Hospede { Nome = "Iago", CPF = "123.123.123-12" };
        await dbContext.Hospedes.AddAsync(hospede);
        await dbContext.SaveChangesAsync();

        var veiculo = new Veiculo(
            placa: "ABC-1234",
            modelo: "Gol",
            cor: "Preto",
            observacao: "Carro do Iago",
            hospede: hospede
        );
        await dbContext.Veiculos.AddAsync(veiculo);
        await dbContext.SaveChangesAsync();


        var faturamento = new Faturamento { ValorTotal = 100, DataPagamento = DateTime.UtcNow };
        await dbContext.Faturamentos.AddAsync(faturamento);
        await dbContext.SaveChangesAsync();

        var ticket = new Ticket { Veiculo = veiculo, FaturamentoId = faturamento.Id, DataEntrada = DateTime.UtcNow };
        await dbContext.Tickets.AddAsync(ticket);
        await dbContext.SaveChangesAsync();

        var vaga = new Vaga(1) { TicketId = ticket.Id };
        await repositorioVaga!.CadastrarAsync(vaga);
        await dbContext.SaveChangesAsync();

        var vagaEditada = new Vaga(2) { TicketId = ticket.Id };

        // Act
        var conseguiuEditar = await repositorioVaga.EditarAsync(vaga.Id, vagaEditada);
        await dbContext.SaveChangesAsync();

        // Assert
        var registroSelecionado = await repositorioVaga.SelecionarRegistroPorIdAsync(vaga.Id);
        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(vagaEditada.Numero, registroSelecionado!.Numero);
    }

    [TestMethod]
    public async Task Deve_Excluir_Vaga_CorretamenteAsync()
    {
        // Arrange
        var hospede = new Hospede { Nome = "Iago", CPF = "123.123.123-12" };
        await dbContext.Hospedes.AddAsync(hospede);
        await dbContext.SaveChangesAsync();

        var veiculo = new Veiculo(
           placa: "ABC-1234",
           modelo: "Gol",
           cor: "Preto",
           observacao: "Carro do Iago",
           hospede: hospede
        );
        await dbContext.Veiculos.AddAsync(veiculo);
        await dbContext.SaveChangesAsync();

        var faturamento = new Faturamento { ValorTotal = 100, DataPagamento = DateTime.UtcNow };
        await dbContext.Faturamentos.AddAsync(faturamento);
        await dbContext.SaveChangesAsync();

        var ticket = new Ticket { Veiculo = veiculo, FaturamentoId = faturamento.Id, DataEntrada = DateTime.UtcNow };
        await dbContext.Tickets.AddAsync(ticket);
        await dbContext.SaveChangesAsync();

        var vaga = new Vaga(1) { TicketId = ticket.Id };
        await repositorioVaga!.CadastrarAsync(vaga);
        await dbContext.SaveChangesAsync();

        // Act
        var conseguiuExcluir = await repositorioVaga.ExcluirAsync(vaga.Id);
        await dbContext.SaveChangesAsync();

        // Assert
        var registroSelecionado = await repositorioVaga.SelecionarRegistroPorIdAsync(vaga.Id);
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Todas_Vagas_CorretamenteAsync()
    {
        // Arrange
        var hospede = new Hospede("Iago", "123.123.123-12");
        var hospede2 = new Hospede("Iago", "123.123.123-19");
        var hospede3 = new Hospede("Iago", "123.123.123-10");

        var veiculo = new Veiculo("ABC-1234", "Gol", "Preto", hospede, null);
        var veiculo2 = new Veiculo("ABC-1239", "Gol", "Preto", hospede2, null);
        var veiculo3 = new Veiculo("ABC-1239", "Gol", "Preto", hospede3, null);

        // Criar Faturamento mínimo para a FK do Ticket
        var faturamento = new Faturamento(DateTime.UtcNow, 0, null);
        var faturamento2 = new Faturamento(DateTime.UtcNow, 10, null);
        var faturamento3 = new Faturamento(DateTime.UtcNow, 20, null);
        await dbContext.Faturamentos.AddRangeAsync(faturamento, faturamento2, faturamento3);
        await dbContext.SaveChangesAsync();

        // Criar Ticket associado ao Veículo e ao Faturamento
        var ticket = new Ticket(veiculo, null)
        {
            FaturamentoId = faturamento.Id,
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
        await dbContext.Tickets.AddRangeAsync(ticket, ticket2, ticket3);
        await dbContext.SaveChangesAsync();

        // Criar múltiplas vagas vinculadas ao Ticket
        var vaga1 = new Vaga(01) { TicketId = ticket.Id }; // Associar diretamente o objeto
        var vaga2 = new Vaga(02) { TicketId = ticket2.Id };
        var vaga3 = new Vaga(03) { TicketId = ticket3.Id };

        await dbContext.Vagas.AddRangeAsync(vaga1, vaga2, vaga3);
        await dbContext.SaveChangesAsync();

        // Limpar tracking
        dbContext.ChangeTracker.Clear();

        // Act
        var vagasRecebidas = await repositorioVaga.SelecionarRegistrosAsync();

        // Assert
        Assert.AreEqual(3, vagasRecebidas.Count);

        var numerosEsperados = new List<int> { 1, 2, 3 };
        var numerosRecebidos = vagasRecebidas.Select(v => v.Numero).OrderBy(n => n).ToList();
        CollectionAssert.AreEqual(numerosEsperados, numerosRecebidos);
    }
}