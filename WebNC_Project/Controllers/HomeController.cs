using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.DAO;

namespace WebNC_Project.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            
            return View();
        }

        public async Task<ActionResult> DemoRoom()
        {
            var result = await RoomDAO.GetAll();
            return PartialView(result.Take(4));
        }

        [ChildActionOnly]
        public async Task<ActionResult> DemoService()
        {
            return PartialView();
        }


        public async Task<PartialViewResult> DemoVoucher()
        {
            var vouchers = await VoucherDAO.GetAvailable();
            return PartialView(vouchers.Take(4));
        }

        [ChildActionOnly]
        public ActionResult About()
        {
            return PartialView();
        }
    }
}