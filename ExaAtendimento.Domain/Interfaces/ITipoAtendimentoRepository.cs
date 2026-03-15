using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Helpers;

namespace ExaAtendimento.Domain.Interfaces
{
    public interface ITipoAtendimentoRepository
    {
        Task AdicionarAsync(TipoAtendimento tipoAtendimento);
        Task AtualizarAsync(TipoAtendimento tipoAtendimento);
        Task RemoverAsync(int id);
        Task<List<TipoAtendimento>> ObterTodosAsync();
        Task<TipoAtendimento?> ObterPorIdAsync(int id);
        Task<TipoAtendimento?> ObterPorDescricaoAsync(string descricao);
        Task<PagedList<TipoAtendimento>> PesquisarListaAsync(TipoAtendimentoRequest req);
        Task<int> GetTotalCountAsync();
    }
}
