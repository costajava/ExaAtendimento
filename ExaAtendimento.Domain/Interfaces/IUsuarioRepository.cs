using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Helpers;

namespace ExaAtendimento.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task AdicionarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task RemoverAsync(int id);
        Task<Usuario?> ObterPorIdAsync(int id);
        Task<Usuario?> ObterPorNomeAsync(string nome);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<Usuario?> ObterPorCodigoSegurancaAsync(string codigoSeguranca);
        Task<List<Usuario>> ObterTodosAsync();
        Task<PagedList<Usuario>> PesquisarListaAsync(UsuarioRequest req);
        Task<int> GetTotalCountAsync();
        Task<bool> ExisteAdminAsync();
    }
}
