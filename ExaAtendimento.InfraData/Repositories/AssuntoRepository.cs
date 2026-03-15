using Microsoft.EntityFrameworkCore;

using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Helpers;
using ExaAtendimento.InfraData.Data;

namespace ExaAtendimento.InfraData.Repositories
{
    public class AssuntoRepository : IAssuntoRepository
    {
        private readonly AppDbContext _context;

        public AssuntoRepository(AppDbContext context)
        {
            _context = context; 
        }

        public async Task AdicionarAsync(Assunto assunto)
        {
            _context.Assuntos.Add(assunto);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Assunto assunto)
        {
            _context.Assuntos.Update(assunto);
            await _context.SaveChangesAsync();  
        }

        public async Task RemoverAsync(int id)
        {
            var assunto = await _context.Assuntos.FindAsync(id);
            if (assunto != null) 
            { 
                _context.Assuntos.Remove(assunto);
                await _context.SaveChangesAsync();  
            }
        }

        public async Task<Assunto?> ObterPorIdAsync(int id)
        {
            return await _context.Assuntos
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Assunto>> ObterTodosAsync()
        {
            return await _context.Assuntos
                                 .AsNoTracking()
                                 .ToListAsync();
        }
        public async Task<PagedList<Assunto>> PesquisarListaAsync(AssuntoRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            if (req.PageNumber < 1) req.PageNumber = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var query = _context.Assuntos.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(req.TipoAssunto))
            {
                query = query.Where(x => x.TipoAssunto.Contains(req.TipoAssunto));
            }

            var total = await query.CountAsync();

            var resultado = await query
                .OrderBy(x => x.Id)
                .Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            return new PagedList<Assunto>
            {
                Itens = resultado,
                TotalCount = total
            };
        }

        public async Task<int> GetTotalCountAsync()
        {
            var total = await _context.Assuntos.AsNoTracking().CountAsync();
            return total;
        }

        public async Task<Assunto?> ObterPorTipoAssuntoAsync(string tipoAssunto)
        {
            return await _context.Assuntos
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.TipoAssunto == tipoAssunto);
        }
    }
}
