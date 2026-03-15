using Microsoft.EntityFrameworkCore;

using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Helpers;
using ExaAtendimento.Domain.Enums;
using ExaAtendimento.InfraData.Data;

namespace ExaAtendimento.InfraData.Repositories
{
    public class AtendimentoRepository : IAtendimentoRepository
    {
        private readonly AppDbContext _context;

        public AtendimentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Atendimento atendimento)
        {
            _context.Atendimentos.Add(atendimento);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Atendimento atendimento)
        {
            _context.Atendimentos.Update(atendimento);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var atendimento = await _context.Atendimentos.FindAsync(id);
            if (atendimento != null) 
            { 
                _context.Atendimentos.Remove(atendimento);
                await _context.SaveChangesAsync();  
            }
        }

        public async Task<Atendimento?> ObterPorIdAsync(int id)
        {
            return await _context.Atendimentos
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Atendimento>> ObterTodosAsync()
        {
            return await _context.Atendimentos
                         .AsNoTracking()
                         .Include(x => x.Ca)
                         .Include(x => x.Cliente)
                         .ToListAsync();
        }

        public async Task<PagedList<Atendimento>> PesquisarListaAsync(AtendimentoRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            if (req.UsuarioId <= 0) throw new ArgumentException("ID do usuário inválido", nameof(req.UsuarioId));
            if (!Enum.IsDefined(typeof(PerfilUsuario), req.Perfil))
            {
                throw new ArgumentException("Perfil de usuário inválido.", nameof(req.Perfil));
            }
            if (req.PageNumber < 1) req.PageNumber = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            // Usa DateOnly somente data sem hora
            // Solução Correta com o operador '??':
            var dataInicial = req.DataInicial ?? DateOnly.FromDateTime(DateTime.Today);
            var dataFinal = req.DataFinal ?? DateOnly.FromDateTime(DateTime.Today);
            var usuarioId = req.UsuarioId;
            var perfil = req.Perfil;
            
            var query = _context.Atendimentos.AsNoTracking()
                    .Include(x => x.Ca)
                    .Include(x => x.Cliente)
                    .Where(x => x.DataAtendimento >= dataInicial &&
                                x.DataAtendimento <= dataFinal);

            if (perfil == PerfilUsuario.Atendente)
            {
                query = query.Where(x => x.UsuarioId == usuarioId);
            }

            var total = await query.CountAsync();

            var resultado = await query
                .OrderBy(x => x.DataAtendimento)
                .Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            return new PagedList<Atendimento>
            {
                Itens = resultado,
                TotalCount = total
            };
        }
        public async Task<int> GetTotalCountAsync()
        {
            var total = await _context.Atendimentos.AsNoTracking().CountAsync();
            return total;
        }
    }
}
