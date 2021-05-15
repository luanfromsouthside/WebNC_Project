using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebNC_Project.Areas.Server.Controllers
{
    public class HomeController : Controller
    {
        // GET: Server/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}