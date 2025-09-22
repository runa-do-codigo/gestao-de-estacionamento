using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using TesteFacil.Testes.Integracao.Compartilhado;

namespace GestaoDeEstacionamento.Testes.Integracao.ModuloHospede;

[TestClass]
[TestCategory("Testes de Integração de Hospede")]
public sealed class RepositorioHospedeEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Hospede_CorretamenteAsync()
    {
        // Arrange
        var hospede = new Hospede("Iago", "123.123.123-12");

        // Act
        repositorioHospede?.CadastrarAsync(hospede);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioHospede?.SelecionarRegistroPorIdAsync(hospede.Id);

        Assert.AreEqual(hospede, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Editar_Hospede_CorretamenteAsync()
    {
        // Arrange
        var hospede = new Hospede("Iago", "123.123.123-12");
        repositorioHospede?.CadastrarAsync(hospede);
        dbContext?.SaveChanges();

        var hospedeEditada = new Hospede("Iago Editado", "123.123.123-12");

        // Act
        var conseguiuEditar = await repositorioHospede?.EditarAsync(hospede.Id, hospedeEditada);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioHospede?.SelecionarRegistroPorIdAsync(hospede.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(hospede, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Excluir_Hospede_CorretamenteAsync()
    {
        // Arrange
        var hospede = new Hospede("Iago", "123.123.123-12");
        repositorioHospede?.CadastrarAsync(hospede);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = await repositorioHospede?.ExcluirAsync(hospede.Id);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioHospede?.SelecionarRegistroPorIdAsync(hospede.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Hospedes_CorretamenteAsync()
    {
        // Arrange - Arranjo
        var hospede = new Hospede("Iago", "123.123.123-12");
        var hospede2 = new Hospede("Gustavo", "321.123.123-12");
        var hospede3 = new Hospede("Pedro", "123.123.123-45");

        List<Hospede> hospedesEsperadas = [hospede, hospede2, hospede3];

        repositorioHospede?.CadastrarEntidades(hospedesEsperadas);
        dbContext?.SaveChanges();

        var hospedesEsperadasOrdenadas = hospedesEsperadas
            .OrderBy(d => d.Nome)
            .ToList();

        // Act - Ação
        var hospedesRecebidas = await repositorioHospede?.SelecionarRegistrosAsync();

        // Assert - Asseção
        CollectionAssert.AreEqual(hospedesEsperadasOrdenadas, hospedesRecebidas);
    }
}