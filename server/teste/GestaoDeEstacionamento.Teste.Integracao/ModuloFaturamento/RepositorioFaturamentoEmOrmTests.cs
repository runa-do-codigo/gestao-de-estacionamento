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
        // Arrange
        var ticket = Builder<Ticket>.CreateNew().Persist();

        var faturamento = new Faturamento(new DateTime(2025-09-21), 20, ticket);

        // Act
        repositorioFaturamento?.CadastrarAsync(faturamento);
        dbContext?.SaveChanges();

        // Assert
        var faturamentoSelecionada = await repositorioFaturamento?.SelecionarRegistroPorIdAsync(faturamento.Id);

        Assert.AreEqual(faturamento, faturamentoSelecionada);
    }

    [TestMethod]
    public async Task Deve_Editar_Faturamento_CorretamenteAsync()
    {
        // Arrange
        var ticket = Builder<Ticket>.CreateNew().Persist();

        var faturamento = new Faturamento(new DateTime(2025-09-21), 20, ticket);
        repositorioFaturamento?.CadastrarAsync(faturamento);
        dbContext?.SaveChanges();

        var faturamentoEditado = new Faturamento(new DateTime(2025-09-21), 30, ticket);

        // Act
        var conseguiuEditar = await repositorioFaturamento?.EditarAsync(faturamento.Id, faturamentoEditado);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioFaturamento?.SelecionarRegistroPorIdAsync(faturamento.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(faturamento, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Excluir_Faturamento_CorretamenteAsync()
    {
        // Arrange
        var ticket = Builder<Ticket>.CreateNew().Persist();

        var faturamento = new Faturamento(new DateTime(2025-09-21), 20, ticket);
        repositorioFaturamento?.CadastrarAsync(faturamento);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = await repositorioFaturamento?.ExcluirAsync(faturamento.Id);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioFaturamento?.SelecionarRegistroPorIdAsync(faturamento.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Faturamentos_CorretamenteAsync()
    {
        // Arrange - Arranjo
        var ticket = Builder<Ticket>.CreateListOfSize(3).Persist().ToList();

        var faturamento = new Faturamento(new DateTime(2025-09-21), 20, ticket[0]);
        var faturamento2 = new Faturamento(new DateTime(2025-09-21), 20, ticket[1]);
        var faturamento3 = new Faturamento(new DateTime(2025-09-21), 20, ticket[2]);

        List<Faturamento> faturamentosEsperadas = [faturamento, faturamento2, faturamento3];

        repositorioFaturamento?.CadastrarEntidades(faturamentosEsperadas);
        dbContext?.SaveChanges();

        var faturamentosEsperadasOrdenadas = faturamentosEsperadas
            .OrderBy(d => d.Id)
            .ToList();

        // Act - Ação
        var faturamentosRecebidas = await repositorioFaturamento?.SelecionarRegistrosAsync();

        // Assert - Asseção
        CollectionAssert.AreEqual(faturamentosEsperadasOrdenadas, faturamentosRecebidas);
    }
    [TestMethod]
    public async Task Deve_Obter_Valor_Faturamentos_CorretamenteAsync()
    {
        // Arrange
        var veiculo = Builder<Veiculo>.CreateNew().Persist();
        var vaga = Builder<Vaga>.CreateNew().Persist();
        var ticket = new Ticket(veiculo, vaga);
        ticket.Saida();

        var faturamento = new Faturamento(new DateTime(2025-09-21), null, ticket);

        // Act 
        repositorioFaturamento?.CadastrarAsync(faturamento);
        repositorioFaturamento?.ObterTotalFatura(faturamento.Id);
        dbContext?.SaveChanges();

        // Assert
        Assert.AreEqual(faturamento.ValorTotal, 10);
    }
}