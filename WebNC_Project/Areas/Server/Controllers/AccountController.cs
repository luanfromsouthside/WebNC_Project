using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.DAO;
using WebNC_Project.ViewModel;

namespace WebNC_Project.Areas.Server.Controllers
{
    public class AccountController : Controller
    {
        // GET: Server/Account
        public ActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = StaffDAO.Instance.GetByID(model.Username);
                if(user == null)
                {
                    ModelState.AddModelError("", "User does not exist");
                    return View(model);
                }
                if (user.Password != model.Password)
                {
                    ModelState.AddModelError("", "Password incorrect");
                    return View(model);
                }
                return RedirectToAction("Index", "Staffs");
            }
            return View(model);
        }

        public PartialViewResult UserInfo()
        {
            return PartialView();
        }
    }
}