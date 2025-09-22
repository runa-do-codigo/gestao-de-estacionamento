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

        var veiculo = new Veiculo("ABC-1234", "Ford", "Branco", hospede, null);

        // Act
        await repositorioVeiculo!.CadastrarAsync(veiculo);
        await dbContext!.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        // Assert
        var veiculoSelecionado = await repositorioVeiculo.SelecionarRegistroPorIdAsync(veiculo.Id);
        Assert.AreEqual(veiculo.Id, veiculoSelecionado!.Id);
    }

    [TestMethod]
    public async Task Deve_Editar_Veiculo_CorretamenteAsync()
    {
        // Arrange
        var hospede = Builder<Hospede>.CreateNew().Persist();
        var veiculo = new Veiculo("ABC-1234", "Ford", "Branco", hospede, null);

        await repositorioVeiculo!.CadastrarAsync(veiculo);
        await dbContext!.SaveChangesAsync();

        var veiculoEditado = new Veiculo("ABC-1234", "Ford Editado", "Branco", hospede, null);

        // Act
        var conseguiuEditar = await repositorioVeiculo.EditarAsync(veiculo.Id, veiculoEditado);
        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        // Assert
        var registroSelecionado = await repositorioVeiculo.SelecionarRegistroPorIdAsync(veiculo.Id);
        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(veiculoEditado.Modelo, registroSelecionado!.Modelo);
    }

    [TestMethod]
    public async Task Deve_Excluir_Veiculo_CorretamenteAsync()
    {
        // Arrange
        var hospede = Builder<Hospede>.CreateNew().Persist();
        var veiculo = new Veiculo("ABC-1234", "Ford", "Branco", hospede, null);

        await repositorioVeiculo!.CadastrarAsync(veiculo);
        await dbContext!.SaveChangesAsync();

        // Act
        var conseguiuExcluir = await repositorioVeiculo.ExcluirAsync(veiculo.Id);
        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        // Assert
        var registroSelecionado = await repositorioVeiculo.SelecionarRegistroPorIdAsync(veiculo.Id);
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Veiculos_CorretamenteAsync()
    {
        // Arrange
        var hospedes = Builder<Hospede>.CreateListOfSize(3).Persist().ToList();

        var veiculo1 = new Veiculo("ABC-1234", "Ford", "Branco", hospedes[0], null);
        var veiculo2 = new Veiculo("BCD-1234", "Ford", "Branco", hospedes[1], null);
        var veiculo3 = new Veiculo("CDE-1234", "Ford", "Branco", hospedes[2], null);

        var veiculosEsperados = new List<Veiculo> { veiculo1, veiculo2, veiculo3 };

        await repositorioVeiculo!.CadastrarEntidades(veiculosEsperados);
        await dbContext!.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        var veiculosEsperadosOrdenados = veiculosEsperados
            .OrderBy(v => v.Placa)
            .ToList();

        // Act
        var veiculosRecebidos = (await repositorioVeiculo.SelecionarRegistrosAsync())
            .OrderBy(v => v.Placa)
            .ToList();

        // Assert
        CollectionAssert.AreEqual(
            veiculosEsperadosOrdenados.Select(v => v.Id).ToList(),
            veiculosRecebidos.Select(v => v.Id).ToList()
        );
    }
}