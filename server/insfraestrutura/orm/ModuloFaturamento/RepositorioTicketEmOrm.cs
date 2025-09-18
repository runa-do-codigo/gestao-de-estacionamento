using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFaturamento;

public class RepositorioTicketEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<Ticket>(contexto), IRepositorioTicket
{ };