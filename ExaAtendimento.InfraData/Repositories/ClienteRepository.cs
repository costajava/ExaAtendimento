using Microsoft.EntityFrameworkCore;

using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Helpers;
using ExaAtendimento.InfraData.Data;

namespace ExaAtendimento.InfraData.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AdicionarAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null) 
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Cliente?> ObterPorIdAsync(int id)
        {
            return await _context.Clientes
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Cliente>> ObterTodosAsync()
        {
            return await _context.Clientes.AsNoTracking().ToListAsync();
        }

        public async Task<List<Cliente>> BuscarPorCaAsync(int caId)
        {
            var clientes = await _context.Clientes
                                 .AsNoTracking()
                                 .Where(x => x.CaId == caId)
                                 .OrderBy(x => x.Nome)
                                 .ToListAsync();

            return clientes;
        }

        public async Task<PagedList<Cliente>> PesquisarListaAsync(ClienteRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            if (req.PageNumber < 1) req.PageNumber = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var query = _context.Clientes.AsNoTracking();

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

            return new PagedList<Cliente>
            {
                Itens = resultado,
                TotalCount = total
            };
        }

        public async Task<int> GetTotalCountAsync()
        {
            var total = await _context.Clientes.AsNoTracking().CountAsync();
            return total;
        }
    }
}
