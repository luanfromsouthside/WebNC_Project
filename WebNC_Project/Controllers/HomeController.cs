using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebNC_Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public async Task<ActionResult> DemoRoom()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public async Task<ActionResult> DemoService()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public async Task<ActionResult> FeedBack()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult About()
        {
            return PartialView();
        }
    }
}