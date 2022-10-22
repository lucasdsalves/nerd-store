using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalog.API.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace NSE.Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("products")]
        [SwaggerOperation("List all products")]
        public async Task<IActionResult> GetAll()
        {
            return new JsonResult(await _productRepository.GetAll());
        }

        [HttpGet("products/{id}")]
        [SwaggerOperation("Obtain product by id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return new JsonResult(await _productRepository.GetById(id));
        }
    }
}
