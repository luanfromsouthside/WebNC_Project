using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.DAO;
using WebNC_Project.Models;

namespace WebNC_Project.Areas.Server.Controllers
{
    public class StaffsController : Controller
    {
        // GET: Server/Staffs
        public ActionResult Index(string search)
        {
            ViewBag.Search = search;
            return View(StaffDAO.Instance.GetAll());
        }
        private void SetListPermis()
        {
            SelectList listPer = new SelectList(PermissionDAO.Instance.GetAll(), "ID", "Name");
            ViewBag.ListPer = listPer;
        }

        public ActionResult Create()
        {
            SetListPermis();
            return View(new Staff());
        }

        [HttpPost]
        public ActionResult Create(Staff staff)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = StaffDAO.Instance.GetByID(staff.ID);
                    if (entity != null) return RedirectToAction("Create");
                    StaffDAO.Instance.Create(staff);
                    return RedirectToAction("Index");
                }
                SetListPermis();
                return View(staff);
            }            
            catch
            {
                SetListPermis();
                ModelState.AddModelError("", "Server can not create staff");
                return View(staff);
            }
        }

        public ActionResult Details(string id)
        {
            var result = StaffDAO.Instance.GetByID(id);
            return View(result);
        }

        public ActionResult Edit(string id)
        {
            SetListPermis();
            var result = StaffDAO.Instance.GetByID(id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(Staff staff)
        {
            SetListPermis();
            try
            {
                if (ModelState.IsValid)
                {
                    StaffDAO.Instance.Edit(staff);
                    return RedirectToAction("Details", new { id = staff.ID.Trim() });
                }
                return View(staff);
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(staff);
            }
        }

        [HttpPost]
        public ActionResult Remove(string id)
        {
            var result = StaffDAO.Instance.GetByID(id);
            if (result == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            try
            {
                StaffDAO.Instance.Remove(id);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}