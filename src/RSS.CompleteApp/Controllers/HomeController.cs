using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RSS.Business.Interfaces;
using RSS.CompleteApp.ViewModels;

namespace RSS.CompleteApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
                                INotifiable notifiable) : base(notifiable)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var erroModel = new ErrorViewModel();

            if (id == 500)
            {
                erroModel.Message = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                erroModel.Title = "Ocorreu um erro!";
                erroModel.ErrorCode = id;
            }
            else if (id == 404)
            {
                erroModel.Message = "A página que está procurando não existe! <br /> Em caso de dúvidas entre em contato com nosso suporte";
                erroModel.Title = "Ops! Página não encontrada.";
                erroModel.ErrorCode = id;
            }
            else if (id == 403)
            {
                erroModel.Message = "Você não tem permissão para fazer isto.";
                erroModel.Title = "Acesso Negado";
                erroModel.ErrorCode = id;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", erroModel);
        }
    }
}
