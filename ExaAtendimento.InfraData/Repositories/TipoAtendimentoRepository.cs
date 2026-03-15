using Microsoft.EntityFrameworkCore;

using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Helpers;
using ExaAtendimento.InfraData.Data;

namespace ExaAtendimento.InfraData.Repositories
{
    public class TipoAtendimentoRepository : ITipoAtendimentoRepository
    {
        private readonly AppDbContext _context;

        public TipoAtendimentoRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AdicionarAsync(TipoAtendimento tipoAtendimento)
        {
            _context.TipoAtendimentos.Add(tipoAtendimento);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(TipoAtendimento tipoAtendimento)
        {
            _context.TipoAtendimentos.Update(tipoAtendimento);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var tipoAtendimento = await _context.TipoAtendimentos.FindAsync(id);
            if (tipoAtendimento != null)
            {
                _context.TipoAtendimentos.Remove(tipoAtendimento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TipoAtendimento?> ObterPorIdAsync(int id)
        {
            return await _context.TipoAtendimentos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<TipoAtendimento>> ObterTodosAsync()
        {
            return await _context.TipoAtendimentos.AsNoTracking().ToListAsync();
        }

        public async Task<PagedList<TipoAtendimento>> PesquisarListaAsync(TipoAtendimentoRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            if (req.PageNumber < 1) req.PageNumber = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var query = _context.TipoAtendimentos.AsNoTracking();

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

            return new PagedList<TipoAtendimento>
            {
                Itens = resultado,
                TotalCount = total
            };
        }

        public async Task<int> GetTotalCountAsync()
        {
            var total = await _context.TipoAtendimentos.AsNoTracking().CountAsync();
            return total;
        }

        public async Task<TipoAtendimento?> ObterPorDescricaoAsync(string descricao)
        {
            return await _context.TipoAtendimentos
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(x => x.Descricao == descricao);
        }
    }
}
