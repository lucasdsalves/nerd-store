using NSE.WebApp.MVC.Models;
using Refit;

namespace NSE.WebApp.MVC.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductViewModel>> GetAll();
        Task<ProductViewModel> GetById(Guid id);
    }
    
    public interface ICatalogServiceRefit
    {
        [Get("/api/catalog/products")]
        Task<IEnumerable<ProductViewModel>> GetAll();

        [Get("/api/catalog/products/{id}")]
        Task<ProductViewModel> GetById(Guid id);
    }
}
