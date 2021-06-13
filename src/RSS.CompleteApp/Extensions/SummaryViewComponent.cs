using Microsoft.AspNetCore.Mvc;
using RSS.Business.Interfaces;
using System.Threading.Tasks;

namespace RSS.CompleteApp.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotifiable _notifiable;

        public SummaryViewComponent(INotifiable notifiable)
        {
            _notifiable = notifiable;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notification = await Task.FromResult(_notifiable.GetNotifications());

            notification.ForEach(n => ViewData.ModelState.AddModelError(string.Empty, n.Message));

            return View();
        }
    }
}
