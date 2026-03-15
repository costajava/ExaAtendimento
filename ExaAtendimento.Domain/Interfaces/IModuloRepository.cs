using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Helpers;

namespace ExaAtendimento.Domain.Interfaces
{
    public interface IModuloRepository
    {
        Task AdicionarAsync(Modulo modulo);
        Task AtualizarAsync(Modulo modulo);
        Task RemoverAsync(int id);
        Task<Modulo?> ObterPorIdAsync(int id);
        Task<Modulo?> ObterPorNomeAsync(string nome);
        Task<bool> ExisteAsync(int id);
        Task<List<Modulo>> ObterTodosAsync();
        Task<PagedList<Modulo>> PesquisarListaAsync(ModuloRequest req);
        Task<int> GetTotalCountAsync();
    }
}
