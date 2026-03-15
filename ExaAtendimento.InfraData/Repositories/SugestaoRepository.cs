using Microsoft.EntityFrameworkCore;

using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Helpers;
using ExaAtendimento.InfraData.Data;

namespace ExaAtendimento.InfraData.Repositories
{
    public class SugestaoRepository : ISugestaoRepository
    {
        private readonly AppDbContext _context;

        public SugestaoRepository(AppDbContext context)
        {
            _context = context; 
        }

        public async Task AdicionarAsync(Sugestao sugestao)
        {
            _context.Sugestoes.Add(sugestao);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Sugestao sugestao)
        {
            _context.Sugestoes.Update(sugestao);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var sugestao = await _context.Sugestoes.FindAsync(id);
            if (sugestao != null) 
            { 
                _context.Sugestoes.Remove(sugestao);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Sugestao?> ObterPorIdAsync(int id)
        {
            return await _context.Sugestoes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Sugestao>> ObterTodosAsync()
        {
            return await _context.Sugestoes.AsNoTracking().ToListAsync();
        }

        public async Task<PagedList<Sugestao>> PesquisarListaAsync(SugestaoRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            if (req.PageNumber < 1) req.PageNumber = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var query = _context.Sugestoes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(req.Descricao))
            {
                query = query.Where(x => x.Descricao.Contains(req.Descricao));
            }

            var total = await query.CountAsync();

            var resultado = await query
                .OrderBy(x => x.Id)
                .Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            return new PagedList<Sugestao>
            {
                Itens = resultado,
                TotalCount = total
            };
        }

        public async Task<int> GetTotalCountAsync()
        {
            var total = await _context.Sugestoes.AsNoTracking().CountAsync();
            return total;
        }

        public async Task<Sugestao?> ObterPorDescricaoAsync(string descricao)
        {
            return await _context.Sugestoes
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(x => x.Descricao == descricao);
        }
    }
}
