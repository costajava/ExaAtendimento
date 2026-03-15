using Microsoft.EntityFrameworkCore;

using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Helpers;
using ExaAtendimento.InfraData.Data;

namespace ExaAtendimento.InfraData.Repositories
{
    public class ModuloRepository : IModuloRepository
    {
        private readonly AppDbContext _context;
        public ModuloRepository(AppDbContext context) 
        { 
            _context = context; 
        }
        public async Task AdicionarAsync(Modulo modulo)
        {
            _context.Modulos.Add(modulo);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Modulo modulo)
        {
            _context.Modulos.Update(modulo);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var modulo = await _context.Modulos.FindAsync(id);
            if (modulo != null)
            {
                _context.Modulos.Remove(modulo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Modulo?> ObterPorIdAsync(int id)
        {
            return await _context.Modulos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Modulos.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Modulo>> ObterTodosAsync()
        {
            return await _context.Modulos.AsNoTracking().ToListAsync();
        }

        public async Task<PagedList<Modulo>> PesquisarListaAsync(ModuloRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            if (req.PageNumber < 1) req.PageNumber = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var query = _context.Modulos.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(req.Nome))
            {
                query = query.Where(x => x.Nome.Contains(req.Nome));
            }

            var total = await query.CountAsync();

            var resultado = await query
                .OrderBy(x => x.Id)
                .Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            return new PagedList<Modulo>
            {
                Itens = resultado,
                TotalCount = total
            };
        }

        public async Task<int> GetTotalCountAsync()
        {
            var total = await _context.Modulos.AsNoTracking().CountAsync();
            return total;
        }

        public async Task<Modulo?> ObterPorNomeAsync(string nome)
        {
            return await _context.Modulos
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(x => x.Nome == nome);
        }
    }
}
