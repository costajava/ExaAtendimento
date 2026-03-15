using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Helpers;

namespace ExaAtendimento.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task AdicionarAsync(Cliente cliente);
        Task AtualizarAsync(Cliente cliente);
        Task RemoverAsync(int id);
        Task<Cliente?> ObterPorIdAsync(int id);
        Task<List<Cliente>> ObterTodosAsync();
        Task<List<Cliente>> BuscarPorCaAsync(int caId);
        Task<PagedList<Cliente>> PesquisarListaAsync(ClienteRequest req);
        Task<int> GetTotalCountAsync();
    }
}
