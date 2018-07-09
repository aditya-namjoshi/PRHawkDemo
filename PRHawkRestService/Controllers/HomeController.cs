using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace PRHawkRestService.Controllers
{
    public class HomeController : Controller
    {
        // Landing page controller
        // The view calls the Username Web API controller on button click
        public ActionResult Index()
        {
            return View();
        }
    }
}
