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
        await repositorioHospede!.CadastrarAsync(hospede);
        await dbContext!.SaveChangesAsync();

        // Limpar rastreamento para evitar conflitos
        dbContext.ChangeTracker.Clear();

        // Assert
        var registroSelecionado = await repositorioHospede.SelecionarRegistroPorIdAsync(hospede.Id);
        Assert.AreEqual(hospede.Id, registroSelecionado!.Id);
    }

    [TestMethod]
    public async Task Deve_Editar_Hospede_CorretamenteAsync()
    {
        // Arrange
        var hospede = new Hospede("Iago", "123.123.123-12");
        await repositorioHospede!.CadastrarAsync(hospede);
        await dbContext!.SaveChangesAsync();

        var hospedeEditado = new Hospede("Iago Editado", "123.123.123-12");

        // Act
        var conseguiuEditar = await repositorioHospede.EditarAsync(hospede.Id, hospedeEditado);
        await dbContext.SaveChangesAsync();

        // Limpar rastreamento para evitar conflitos
        dbContext.ChangeTracker.Clear();

        // Assert
        var registroSelecionado = await repositorioHospede.SelecionarRegistroPorIdAsync(hospede.Id);
        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(hospedeEditado.Nome, registroSelecionado!.Nome);
    }

    [TestMethod]
    public async Task Deve_Excluir_Hospede_CorretamenteAsync()
    {
        // Arrange
        var hospede = new Hospede("Iago", "123.123.123-12");
        await repositorioHospede!.CadastrarAsync(hospede);
        await dbContext!.SaveChangesAsync();

        // Act
        var conseguiuExcluir = await repositorioHospede.ExcluirAsync(hospede.Id);
        await dbContext.SaveChangesAsync();

        // Limpar rastreamento para evitar conflitos
        dbContext.ChangeTracker.Clear();

        // Assert
        var registroSelecionado = await repositorioHospede.SelecionarRegistroPorIdAsync(hospede.Id);
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Todos_Hospedes_CorretamenteAsync()
    {
        // Arrange
        var hospede1 = new Hospede("Lucas", "111.111.111-11");
        var hospede2 = new Hospede("Maria", "222.222.222-22");
        var hospede3 = new Hospede("João", "333.333.333-33");

        // Adiciona e salva cada entidade
        await repositorioHospede!.CadastrarAsync(hospede1);
        await repositorioHospede!.CadastrarAsync(hospede2);
        await repositorioHospede!.CadastrarAsync(hospede3);
        await dbContext.SaveChangesAsync();

        // Limpa rastreamento para evitar conflito
        dbContext.ChangeTracker.Clear();

        // Act
        var hospedesRecebidos = await repositorioHospede.SelecionarRegistrosAsync();

        // Assert
        var nomesEsperados = new List<string> { hospede1.Nome, hospede2.Nome, hospede3.Nome }
            .OrderBy(n => n).ToList();
        var nomesRecebidos = hospedesRecebidos!.Select(h => h.Nome).OrderBy(n => n).ToList();

        CollectionAssert.AreEqual(nomesEsperados, nomesRecebidos);
    }
}