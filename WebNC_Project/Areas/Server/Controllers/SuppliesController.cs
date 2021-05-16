using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.Models;
using WebNC_Project.DAO;
using WebNC_Project.ViewModel;

namespace WebNC_Project.Areas.Server.Controllers
{
    public class SuppliesController : Controller
    {
        // GET: Server/Supplies
        public ActionResult Index(string search)
        {
            ViewBag.Search = search;
            return View(SupplyDAO.Instance.GetAll());
        }

        // GET: Server/Supplies/Details/5
        public ActionResult Details(string id)
        {
            var result = SupplyDAO.Instance.GetByID(id);
            return View(result);
        }

        // GET: Server/Supplies/Create
        public ActionResult Create()
        {
            return View(new Supply());
        }

        // POST: Server/Supplies/Create
        [HttpPost]
        public ActionResult Create(Supply sup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = SupplyDAO.Instance.GetByID(sup.ID);
                    if (entity != null)
                    {
                        ModelState.AddModelError("ID", "ID was exist, try again with new ID");
                        return View(sup);
                    }
                    SupplyDAO.Instance.Create(sup);
                    return RedirectToAction("Index");
                }
                return View(sup);
            }
            catch
            {
                ModelState.AddModelError("", "Server can not create supply");
                return View(sup);
            }
        }

        // GET: Server/Supplies/Edit/5
        public ActionResult Edit(string id)
        {
            Supply result = SupplyDAO.Instance.GetByID(id);
            return View(result);
        }

        // POST: Server/Supplies/Edit/5
        [HttpPost]
        public ActionResult Edit(Supply model, string EditType, int? Count)
        {
            try
            {
                if (Count < 0 && EditType != "none")
                {
                    ModelState.AddModelError("Count", "Enter a valid number");
                    return View(model);
                } 
                if (ModelState.IsValid)
                {
                    SupplyDAO.Instance.Edit(model,EditType,Count);
                    return RedirectToAction("Details", new { id = model.ID.Trim() });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // POST: Server/Supplies/Delete/5
        [HttpPost]
        public ActionResult Remove(string id)
        {
            var result = SupplyDAO.Instance.GetByID(id);
            if (result == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            try
            {
                SupplyDAO.Instance.Remove(id);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
