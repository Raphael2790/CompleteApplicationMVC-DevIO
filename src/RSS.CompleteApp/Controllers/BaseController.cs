using KissLog;
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
        private readonly ILogger _logger;

        public BaseController(INotifiable notifiable, ILogger logger)
        {
            _notifiable = notifiable;
            _logger = logger;
        }

        protected bool ValidOperation()
        {
            return !_notifiable.HasNotification();
        }
    }
}
