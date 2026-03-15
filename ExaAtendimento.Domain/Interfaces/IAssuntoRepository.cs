using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Helpers;

namespace ExaAtendimento.Domain.Interfaces
{
    public interface IAssuntoRepository
    {
        Task AdicionarAsync(Assunto assunto);
        Task AtualizarAsync(Assunto assunto);
        Task RemoverAsync(int id);
        Task<Assunto?> ObterPorIdAsync(int id);
        Task<Assunto?> ObterPorTipoAssuntoAsync(string tipoAssunto);
        Task<List<Assunto>> ObterTodosAsync();
        Task<PagedList<Assunto>> PesquisarListaAsync(AssuntoRequest req);
        Task<int> GetTotalCountAsync();
    }
}
