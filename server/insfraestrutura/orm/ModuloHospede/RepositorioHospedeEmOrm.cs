using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloHospede;
public class RepositorioHospedeEmOrm(AppDbContext contexto) : RepositorioBaseEmOrm<Hospede>(contexto), IRepositorioHospede;