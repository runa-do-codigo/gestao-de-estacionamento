using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVeiculo;
public class RepositorioVeiculoEmOrm(AppDbContext contexto) 
    : RepositorioBaseEmOrm<Veiculo>(contexto), IRepositorioVeiculo
{
    public override async Task<List<Veiculo>> SelecionarRegistrosAsync()
    {
        return await registros.Include(x => x.Hospede).ToListAsync();
    }

    public override async Task<Veiculo?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return await registros.Include(x => x.Hospede).FirstOrDefaultAsync(x => x.Id == idRegistro);
    }

    public async Task<Veiculo> CadastrarObservacao(Guid veiculoId, string observacao)
    {
        // Buscar veículo pelo Id
        var veiculo = await registros.FirstOrDefaultAsync(v => v.Id == veiculoId);

        // Adicionar a observação (concatena se já existir)
        if (string.IsNullOrWhiteSpace(veiculo.Observacao))
            veiculo.Observacao = observacao;
        else
            veiculo.Observacao += Environment.NewLine + observacao;

        // Salvar alterações
        await contexto.SaveChangesAsync();

        return veiculo;
    }
}