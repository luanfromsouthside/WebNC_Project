using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.Models;
using WebNC_Project.DAO;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WebNC_Project.Areas.Server.Controllers
{
    public class SuppliesController : Controller
    {
        // GET: Server/Supplies
        public async Task<ActionResult> Index(string search)
        {
            ViewBag.Search = search;
            return View(await SupplyDAO.GetAll());
        }

        // GET: Server/Supplies/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var result = SupplyDAO.GetByID(id);
            return View(await result);
        }

        // GET: Server/Supplies/Create
        public ActionResult Create()
        {
            return View(new Supply());
        }

        // POST: Server/Supplies/Create
        [HttpPost]
        public async Task<ActionResult> Create(Supply sup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await SupplyDAO.GetByID(sup.ID);
                    if (entity != null)
                    {
                        ModelState.AddModelError("ID", "ID was exist, try again with new ID");
                        return View(sup);
                    }
                    await SupplyDAO.Create(sup);
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
        public async Task<ActionResult> Edit(string id)
        {
            Supply result = await SupplyDAO.GetByID(id);
            return View(result);
        }

        // POST: Server/Supplies/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Supply model, string EditType, int? Count)
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
                    await SupplyDAO.Edit(model,EditType,Count);
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
        public async Task<ActionResult> Remove(string id)
        {
            var result = await SupplyDAO.GetByID(id);
            if (result == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            try
            {
                await SupplyDAO.Remove(id);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
