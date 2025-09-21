using FizzWare.NBuilder;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using TesteFacil.Testes.Integracao.Compartilhado;

namespace GestaoDeEstacionamento.Testes.Integracao.ModuloVeiculo;

[TestClass]
[TestCategory("Testes de Integração de Veiculo")]
public sealed class RepositorioVeiculoEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Veiculo_CorretamenteAsync()
    {
        // Arrange
        var hospede = Builder<Hospede>.CreateNew().Persist();

        var veiculo = new Veiculo("ABC-12345", "Ford", "Branco", hospede);

        // Act
        repositorioVeiculo?.CadastrarAsync(veiculo);
        dbContext?.SaveChanges();

        // Assert
        var veiculoSelecionada = await repositorioVeiculo?.SelecionarRegistroPorIdAsync(veiculo.Id);

        Assert.AreEqual(veiculo, veiculoSelecionada);
    }

    [TestMethod]
    public async Task Deve_Editar_Veiculo_CorretamenteAsync()
    {
        // Arrange
        var hospede = Builder<Hospede>.CreateNew().Persist();

        var veiculo = new Veiculo("ABC-12345", "Ford", "Branco", hospede);
        repositorioVeiculo?.CadastrarAsync(veiculo);
        dbContext?.SaveChanges();

        var veiculoEditado = new Veiculo("ABC-12345", "Ford Editado", "Branco", hospede);

        // Act
        var conseguiuEditar = await repositorioVeiculo?.EditarAsync(veiculo.Id, veiculoEditado);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioVeiculo?.SelecionarRegistroPorIdAsync(veiculo.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(veiculo, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Excluir_Veiculo_CorretamenteAsync()
    {
        // Arrange
        var hospede = Builder<Hospede>.CreateNew().Persist();

        var veiculo = new Veiculo("ABC-12345", "Ford", "Branco", hospede);
        repositorioVeiculo?.CadastrarAsync(veiculo);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = await repositorioVeiculo?.ExcluirAsync(veiculo.Id);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioVeiculo?.SelecionarRegistroPorIdAsync(veiculo.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Veiculos_CorretamenteAsync()
    {
        // Arrange - Arranjo
        var hospede = Builder<Hospede>.CreateListOfSize(3).Persist().ToList();

        var veiculo = new Veiculo("ABC-12345", "Ford", "Branco", hospede[0]);
        var veiculo2 = new Veiculo("BCD-12345", "Ford", "Branco", hospede[1]);
        var veiculo3 = new Veiculo("CDE-12345", "Ford", "Branco", hospede[3]);

        List<Veiculo> veiculosEsperadas = [veiculo, veiculo2, veiculo3];

        repositorioVeiculo?.CadastrarEntidades(veiculosEsperadas);
        dbContext?.SaveChanges();

        var veiculcosEsperadasOrdenadas = veiculosEsperadas
            .OrderBy(d => d.Placa)
            .ToList();

        // Act - Ação
        var veiculosRecebidas = await repositorioVeiculo?.SelecionarRegistrosAsync();

        // Assert - Asseção
        CollectionAssert.AreEqual(veiculcosEsperadasOrdenadas, veiculosRecebidas);
    }
}
