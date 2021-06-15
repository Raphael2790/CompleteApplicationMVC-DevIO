using KissLog;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RSS.CompleteApp.Extensions.Filters
{
    //Confiuração para relaizar log do usuário para cada interação com a aplicação
    public class LogAccessFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public LogAccessFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var message = context.HttpContext.User.Identity.Name + " Acessou : "
                    + context.HttpContext.Request.GetDisplayUrl();

                _logger.Info(message);
            }
        }
    }
}
