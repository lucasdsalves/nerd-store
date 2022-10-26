using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalog.API.Models;
using Swashbuckle.AspNetCore.Annotations;
using static NSE.WebAPI.Core.Identities.CustomAuthorization;

namespace NSE.Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [AllowAnonymous]
        [HttpGet("products")]
        [SwaggerOperation("List all products")]
        public async Task<IActionResult> GetAll()
        {
            return new JsonResult(await _productRepository.GetAll());
        }

        [ClaimsAuthorize("Catalog", "Read")]
        [HttpGet("products/{id}")]
        [SwaggerOperation("Obtain product by id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return new JsonResult(await _productRepository.GetById(id));
        }
    }
}
