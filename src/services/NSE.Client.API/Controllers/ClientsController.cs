using Microsoft.AspNetCore.Mvc;
using NSE.Client.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Client.API.Controllers
{
    [Route("api/[controller]")]
    public class ClientsController : BaseController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClientsController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("clients")]
        public async Task<IActionResult> Index()
        {
            var result = await _mediatorHandler.SendCommand(
                new RegisterClientCommand(Guid.NewGuid(), "Joe", "test@test.com", "78200291022"));

            return CustomResponse(result);
        }
    }
}
