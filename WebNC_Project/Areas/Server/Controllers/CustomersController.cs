using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using WebNC_Project.Models;
using WebNC_Project.DAO;

namespace WebNC_Project.Areas.Server.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Server/Customers
        public ActionResult Index(string search)
        {
            ViewBag.Search = search;
            return View(CustomerDAO.Instance.GetAll());
        }

        // GET: Server/Customers/Details/5
        public ActionResult Details(string id)
        {
            Customer cus = CustomerDAO.Instance.GetByID(id);
            return View(cus);
        }

        // GET: Server/Customers/Create
        public ActionResult Create()
        {
            return View(new Customer());
        }

        // POST: Server/Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = CustomerDAO.Instance.GetByID(customer.ID);
                    if (entity != null)
                    {
                        ModelState.AddModelError("", $"The username {customer.ID} existed");
                        return View(customer);
                    }
                    CustomerDAO.Instance.Create(customer);
                    return RedirectToAction("Index");
                }
                return View(customer);
            }
            catch
            {
                ModelState.AddModelError("", "Server can not create customer");
                return View(customer);
            }
        }

        // GET: Server/Customers/Edit/5
        public ActionResult Edit(string id)
        {
            Customer cus = CustomerDAO.Instance.GetByID(id);
            return View(cus);
        }

        // POST: Server/Customers/Edit/5
        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CustomerDAO.Instance.Edit(customer);
                    return RedirectToAction("Details", new { id = customer.ID.Trim() });
                }
                return View(customer);
            }
            catch
            {
                ModelState.AddModelError("", "Server can not update customer");
                return View();
            }
        }


        // POST: Server/Customers/Delete/5
        [HttpPost]
        public ActionResult Remove(string id)
        {
            var result = CustomerDAO.Instance.GetByID(id);
            if (result == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            try
            {
                CustomerDAO.Instance.Remove(id);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
