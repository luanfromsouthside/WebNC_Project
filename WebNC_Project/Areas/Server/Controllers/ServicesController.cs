using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.DAO;
using WebNC_Project.Models;

namespace WebNC_Project.Areas.Server.Controllers
{
    public class ServicesController : Controller
    {
        // GET: Server/Services
        public ActionResult Index(string search)
        {
            ViewBag.Search = search;
            return View(ServiceDAO.Instance.GetAll());
        }

        // GET: Server/Services/Details/5
        public ActionResult Details(string id)
        {
            Service result = ServiceDAO.Instance.GetByID(id);
            return View(result);
        }

        // GET: Server/Services/Create
        public ActionResult Create()
        {
            return View(new Service());
        }

        // POST: Server/Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Service service)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = ServiceDAO.Instance.GetByID(service.ID);
                    if (entity != null)
                    {
                        ModelState.AddModelError(service.ID, $"The services {service.ID} existed");
                        return View(service);
                    }
                    ServiceDAO.Instance.Create(service);
                    return RedirectToAction("Index");
                }
                return View(service);
            }
            catch
            {
                ModelState.AddModelError("", "Server can not create services");
                return View(service);
            }
        }

        // GET: Server/Services/Edit/5
        public ActionResult Edit(string id)
        {
            Service service = ServiceDAO.Instance.GetByID(id);
            return View(service);
        }

        // POST: Server/Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Service service)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceDAO.Instance.Edit(service);
                    return RedirectToAction("Details", new { id = service.ID.Trim() });
                }
                return View(service);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(service);
            }
        }

        // POST: Server/Services/Delete/5
        [HttpPost]
        public ActionResult Remove(string id)
        {
            var result = ServiceDAO.Instance.GetByID(id);
            if (result == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            try
            {
                ServiceDAO.Instance.Remove(id);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
