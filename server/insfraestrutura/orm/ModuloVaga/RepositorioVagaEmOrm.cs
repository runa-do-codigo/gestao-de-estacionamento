using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVaga;
public class RepositorioVagaEmOrm(AppDbContext contexto) : RepositorioBaseEmOrm<Vaga>(contexto), IRepositorioVaga;