using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Helpers;

namespace ExaAtendimento.Domain.Interfaces
{
    public interface ICaRepository
    {
        Task AdicionarAsync(Ca ca);
        Task AtualizarAsync(Ca ca);
        Task RemoverAsync(int id);
        Task<Ca?> ObterPorIdAsync(int id);
        Task<List<Ca>> ObterTodosAsync();
        Task<List<Ca>> BuscarPorNomeAsync(string nome);
        Task<PagedList<Ca>> PesquisarListaAsync(CaRequest req);
        Task<int> GetTotalCountAsync();
    }
}
