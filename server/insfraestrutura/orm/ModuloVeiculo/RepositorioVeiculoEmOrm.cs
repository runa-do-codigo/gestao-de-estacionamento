using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVeiculo;
public class RepositorioVeiculoEmOrm(AppDbContext contexto) : RepositorioBaseEmOrm<Veiculo>(contexto), IRepositorioVeiculo;