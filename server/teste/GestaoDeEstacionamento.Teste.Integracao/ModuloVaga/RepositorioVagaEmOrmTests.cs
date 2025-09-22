using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using TesteFacil.Testes.Integracao.Compartilhado;

namespace GestaoDeEstacionamento.Testes.Integracao.ModuloVaga;

[TestClass]
[TestCategory("Testes de Integração da Vaga")]
public sealed class RepositorioVagaEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Vaga_CorretamenteAsync()
    {
        // Arrange
        var vaga = new Vaga(01);

        // Act
        repositorioVaga?.CadastrarAsync(vaga);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioVaga?.SelecionarRegistroPorIdAsync(vaga.Id);

        Assert.AreEqual(vaga, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Editar_Vaga_CorretamenteAsync()
    {
        // Arrange
        var vaga = new Vaga(01);
        repositorioVaga?.CadastrarAsync(vaga);
        dbContext?.SaveChanges();

        var vagaEditada= new Vaga(02);

        // Act
        var conseguiuEditar = await repositorioVaga?.EditarAsync(vaga.Id, vagaEditada);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioVaga?.SelecionarRegistroPorIdAsync(vaga.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(vaga, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Excluir_Vaga_CorretamenteAsync()
    {
        // Arrange
        var vaga = new Vaga(01);
        repositorioVaga?.CadastrarAsync(vaga);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = await repositorioVaga?.ExcluirAsync(vaga.Id);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioVaga?.SelecionarRegistroPorIdAsync(vaga.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Vagas_CorretamenteAsync()
    {
        // Arrange - Arranjo
        var vaga = new Vaga(01);
        var vaga2 = new Vaga(02);
        var vaga3 = new Vaga(03);

        List<Vaga> vagasEsperadas = [vaga, vaga2, vaga3];

        repositorioVaga?.CadastrarEntidades(vagasEsperadas);
        dbContext?.SaveChanges();

        var vagasEsperadasOrdenadas = vagasEsperadas
            .OrderBy(d => d.Numero)
            .ToList();

        // Act - Ação
        var vagasRecebidas = await repositorioVaga?.SelecionarRegistrosAsync();

        // Assert - Asseção
        CollectionAssert.AreEqual(vagasEsperadasOrdenadas, vagasRecebidas);
    }
}
