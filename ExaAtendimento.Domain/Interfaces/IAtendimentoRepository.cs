using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Helpers;

namespace ExaAtendimento.Domain.Interfaces
{
    public interface IAtendimentoRepository
    {
        Task AdicionarAsync(Atendimento atendimento);
        Task AtualizarAsync(Atendimento atendimento);
        Task RemoverAsync(int id);
        Task<Atendimento?> ObterPorIdAsync(int id);
        Task<List<Atendimento>> ObterTodosAsync();
        Task<PagedList<Atendimento>> PesquisarListaAsync(AtendimentoRequest req);
        Task<int> GetTotalCountAsync();
    }
}
