using Microsoft.EntityFrameworkCore;

using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Helpers;
using ExaAtendimento.Domain.Enums;
using ExaAtendimento.InfraData.Data;

namespace ExaAtendimento.InfraData.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AdicionarAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Usuario?> ObterPorIdAsync(int id)
        {
            return await _context.Usuarios
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Usuario?> ObterPorNomeAsync(string nome)
        {
            return await _context.Usuarios
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.Nome == nome);
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _context.Usuarios
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Usuario?> ObterPorCodigoSegurancaAsync(string codigoSeguranca)
        {
            return await _context.Usuarios
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.ResetSenhaToken == codigoSeguranca);
        }

        public async Task<List<Usuario>> ObterTodosAsync()
        {
            return await _context.Usuarios
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<PagedList<Usuario>> PesquisarListaAsync(UsuarioRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            if (req.PageNumber < 1) req.PageNumber = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var query = _context.Usuarios.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(req.Nome))
            {
                query = query.Where(x => x.Nome.Contains(req.Nome));
            }

            var total = await query.CountAsync();

            var resultado = await query
                .OrderBy(x => x.Nome)
                .Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            return new PagedList<Usuario>
            {
                Itens = resultado,
                TotalCount = total
            };
        }

        public async Task<int> GetTotalCountAsync()
        {
            var total = await _context.Usuarios.AsNoTracking().CountAsync();
            return total;
        }

        public async Task<bool> ExisteAdminAsync()
        {
            var usuarioAdmin = await _context.Usuarios
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.Perfil == PerfilUsuario.Administrador );
            return usuarioAdmin != null;
        }
    }
}