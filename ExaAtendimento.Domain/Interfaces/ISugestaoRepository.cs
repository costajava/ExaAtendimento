using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Helpers;

namespace ExaAtendimento.Domain.Interfaces
{
    public interface ISugestaoRepository
    {
        Task AdicionarAsync(Sugestao sugestao);
        Task AtualizarAsync(Sugestao sugestao);
        Task RemoverAsync(int id);
        Task<Sugestao?> ObterPorIdAsync(int id);
        Task<Sugestao?> ObterPorDescricaoAsync(string descricao);
        Task<List<Sugestao>> ObterTodosAsync();
        Task<PagedList<Sugestao>> PesquisarListaAsync(SugestaoRequest req);
        Task<int> GetTotalCountAsync();
    }
}
