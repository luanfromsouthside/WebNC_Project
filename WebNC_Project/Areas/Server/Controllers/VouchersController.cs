using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.Models;
using WebNC_Project.DAO;

namespace WebNC_Project.Areas.Server.Controllers
{
    public class VouchersController : Controller
    {
        // GET: Server/Vouchers
        public ActionResult Index(string search)
        {
            ViewBag.Search = search;
            return View(VoucherDAO.Instance.GetAll());
        }

        // GET: Server/Vouchers/Details/5
        public ActionResult Details(string id)
        {
            var result = VoucherDAO.Instance.GetByID(id);
            return View(result);
        }

        // GET: Server/Vouchers/Create
        public ActionResult Create()
        {
            return View(new Voucher());
        }

        // POST: Server/Vouchers/Create
        [HttpPost]
        public ActionResult Create(Voucher voucher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(CheckDate(voucher.FromDate, voucher.ToDate))
                    {
                        ModelState.AddModelError("ToDate", "The date end of voucher was not valid");
                        return View(voucher);
                    }
                    if (voucher.FromDate.Date <= DateTime.Today.Date)
                    {
                        ModelState.AddModelError("FromDate", "The date begin of voucher cannot smaller than to day");
                        return View(voucher);
                    }
                    var entity = VoucherDAO.Instance.GetByID(voucher.Code);
                    if (entity != null)
                    {
                        ModelState.AddModelError("Code", "Code was exist, try again with new Code");
                        return View(voucher);
                    }
                    VoucherDAO.Instance.Create(voucher);
                    return RedirectToAction("Index");
                }
                return View(voucher);
            }
            catch
            {
                ModelState.AddModelError("", "Server can not create supply");
                return View(voucher);
            }
        }

        // GET: Server/Vouchers/Edit/5
        public ActionResult Edit(string id)
        {
            var result = VoucherDAO.Instance.GetByID(id);
            return View(result);
        }

        // POST: Server/Vouchers/Edit/5
        [HttpPost]
        public ActionResult Edit(Voucher voucher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (CheckDate(voucher.FromDate, voucher.ToDate))
                    {
                        ModelState.AddModelError("ToDate", "The date end of voucher was not valid");
                        return View(voucher);
                    }
                    if(voucher.FromDate.Date <= DateTime.Today.Date)
                    {
                        ModelState.AddModelError("FromDate", "The date begin of voucher cannot smaller than to day");
                        return View(voucher);
                    }
                    VoucherDAO.Instance.Edit(voucher);
                    return RedirectToAction("Details", new { id = voucher.Code.Trim() });
                }
                return View(voucher);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(voucher);
            }
        }

        // POST: Server/Vouchers/Delete/5
        [HttpPost]
        public ActionResult Remove(string id)
        {
            var result = VoucherDAO.Instance.GetByID(id);
            if (result == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            try
            {
                VoucherDAO.Instance.Remove(id);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }

        private bool CheckDate(DateTime from, DateTime to)
        {
            return from.Date > to.Date ? false : true;
        }
    }
}
