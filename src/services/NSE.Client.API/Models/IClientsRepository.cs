using NSE.Core.Data;

namespace NSE.Client.API.Models
{
    public interface IClientsRepository : IRepository<Clients>
    {
        void Add(Clients client);
        Task<IEnumerable<Clients>> GetAll();
        Task<Clients> GetByCpf(string cpf);
    }
}
