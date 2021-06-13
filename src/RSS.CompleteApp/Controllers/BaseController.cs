using Microsoft.AspNetCore.Mvc;
using RSS.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSS.CompleteApp.Controllers
{
    public class BaseController : Controller
    {
        private readonly INotifiable _notifiable;

        public BaseController(INotifiable notifiable)
        {
            _notifiable = notifiable;
        }

        protected bool ValidOperation()
        {
            return !_notifiable.HasNotification();
        }
    }
}
