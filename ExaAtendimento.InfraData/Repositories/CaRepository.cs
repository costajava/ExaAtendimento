using Microsoft.EntityFrameworkCore;

using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Helpers;
using ExaAtendimento.InfraData.Data;

namespace ExaAtendimento.InfraData.Repositories
{
    public class CaRepository : ICaRepository
    {
        private readonly AppDbContext _context;

        public CaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Ca ca)
        {
            _context.Cas.Add(ca);
            await _context.SaveChangesAsync();  
        }

        public async Task AtualizarAsync(Ca ca)
        {
            _context.Cas.Update(ca);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var ca = await _context.Cas.FindAsync(id);
            if (ca != null) 
            { 
                _context.Cas.Remove(ca);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Ca?> ObterPorIdAsync(int id)
        {
            return await _context.Cas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Ca>> ObterTodosAsync()
        {
            return await _context.Cas.AsNoTracking().ToListAsync();
        }

        public async Task<PagedList<Ca>> PesquisarListaAsync(CaRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            if (req.PageNumber < 1) req.PageNumber = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var query = _context.Cas.AsNoTracking();

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

            return new PagedList<Ca>
            {
                Itens = resultado,
                TotalCount = total
            };
        }

        public async Task<List<Ca>> BuscarPorNomeAsync(string nome)
        {
            var query = _context.Cas.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.Nome.Contains(nome));
            }

            var resultado = await query
                .OrderBy(x => x.Nome)
                .ToListAsync();

            return resultado;
        }

        public async Task<int> GetTotalCountAsync()
        {
            var total = await _context.Cas.AsNoTracking().CountAsync();
            return total;
        }
    }
}
