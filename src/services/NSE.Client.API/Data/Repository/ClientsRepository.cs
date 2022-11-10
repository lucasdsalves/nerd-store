using Microsoft.EntityFrameworkCore;
using NSE.Client.API.Models;
using NSE.Core.Data;

namespace NSE.Client.API.Data.Repository
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly ClientsContext _context;

        public ClientsRepository(ClientsContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Clients client)
        {
            _context.Clients.Add(client);
        }

        public async Task<IEnumerable<Clients>> GetAll()
        {
            return await _context.Clients.AsNoTracking().ToListAsync();
        }

        public async Task<Clients> GetByCpf(string cpf)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Cpf.Number == cpf);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
