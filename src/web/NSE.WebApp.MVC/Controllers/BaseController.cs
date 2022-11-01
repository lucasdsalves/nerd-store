using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers
{
    public class BaseController : Controller
    {
        protected bool HasResponseErrors(ResponseResult response)
        {
            if (response != null && response.Errors.Messages.Any())
            {
                foreach (var mensagem in response.Errors.Messages)
                {
                    ModelState.AddModelError(string.Empty, mensagem);
                }

                return true;
            }

            return false;
        }
    }
}
