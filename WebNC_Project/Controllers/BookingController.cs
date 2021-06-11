using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stripe.Infrastructure;
using System.Web.Mvc;
using System.Threading.Tasks;
using WebNC_Project.Models;
using WebNC_Project.DAO;
using WebNC_Project.App_Start;
using Stripe.Checkout;

namespace WebNC_Project.Controllers
{
    [CustomerAuthentication]
    public class BookingController : Controller
    {
        // GET: Booking
        public async Task<ActionResult> Index(bool? payment, int? id)
        {
            if(payment.HasValue)
            {
                ViewBag.Stt = true;
                ViewBag.Msg = "Payment success invoice #" + id.Value;
            }
            if (!payment.HasValue)
            {
                ViewBag.Stt = false;
                ViewBag.Msg = "Failed to payment";
            }
            ViewBag.API = "pk_test_51J08eWItPwG3qPODYiIl6BfdFV2GdubPj6cYjvUJqSyFamWLkdAnc6Ynoe4siIdDiTLOMAgJh253Tk1Jyjg735gP00F2JYMAgJ";
            var result = await BookingDAO.BookingsOfCustomer((string)Session["Customer"]);
            return View(result);
        }

        public async Task<ActionResult> Create(string room)
        {
            ViewBag.ListRoom = await ListRoom();
            DateTime today = DateTime.Now.AddDays(1);
            Booking booking = new Booking()
            {
                CustomerID = (string)Session["Customer"],
                RoomID = room,
                CheckinDate = today.Date,
                CheckoutDate = today.AddDays(1).Date
            };
            return View(booking);
        }

        [HttpPost]
        [Obsolete]
        public async Task<ActionResult> Create(Booking model)
        {
            ViewBag.ListRoom = await ListRoom();
            if (!ModelState.IsValid) return View(model);
            if(!(await CustomerDAO.CanBooking((string)Session["Customer"])))
            {
                ModelState.AddModelError("RoomID", "You already have a order and can not book more");
                return View(model);
            }
            if(model.CheckinDate.Date >= model.CheckoutDate.Date)
            {
                ModelState.AddModelError("CheckinDate", "Check in date must greater than check out date");
                return View(model);
            }
            if(!(await RoomDAO.CheckRoom(model.RoomID, model.CheckinDate, model.CheckoutDate)))
            {
                ModelState.AddModelError("RoomID", $"Can not book this room from {model.CheckinDate.ToString("dd/MM/yyyy")} to {model.CheckoutDate.ToString("dd/MM/yyyy")}");
                return View(model);
            }
            var room = await RoomDAO.GetByID(model.RoomID);
            if (room.Adult < model.Adult)
            {
                ModelState.AddModelError("Adult", $"Max adult of room {model.RoomID} is {room.Adult}");
                return View(model);
            }
            if (room.Child < model.Child)
            {
                ModelState.AddModelError("Child", $"Max child of room {model.RoomID} is {room.Child}");
                return View(model);
            }
            try
            {
                await BookingDAO.Create(model);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError("RoomID", "An exception when create your booking request");
                return View(model);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.ID = id;
            ViewBag.ListRoom = await ListRoom();
            var result = await BookingDAO.GetByID(id);
            return PartialView(result);
        }

        [HttpPost]
        [Obsolete]
        public async Task<ActionResult> Edit(Booking model)
        {
            ViewBag.ListRoom = await ListRoom();
            if (!ModelState.IsValid) return PartialView(model);
            if (!(await RoomDAO.CheckRoom(model.RoomID, model.CheckinDate, model.CheckoutDate, model.ID)))
            {
                ModelState.AddModelError("RoomID", $"Can not book this room from {model.CheckinDate.ToString("dd/MM/yyyy")} to {model.CheckoutDate.ToString("dd/MM/yyyy")}");
                return PartialView(model);
            }
            if (model.CheckinDate.Date >= model.CheckoutDate.Date)
            {
                ModelState.AddModelError("CheckinDate", "Check in date must greater than check out date");
                return PartialView(model);
            }
            var room = await RoomDAO.GetByID(model.RoomID);
            if (room.Adult < model.Adult)
            {
                ModelState.AddModelError("Adult", $"Max adult of room {model.RoomID} is {room.Adult}");
                return PartialView(model);
            }
            if (room.Child < model.Child)
            {
                ModelState.AddModelError("Child", $"Max child of room {model.RoomID} is {room.Child}");
                return PartialView(model);
            }
            try
            {
                await BookingDAO.Edit(model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                ModelState.AddModelError("", $"Error when edit information of booking #{model.ID}");
                return PartialView(model);
            }
        }

        public async Task<ActionResult> CallServices(int id)
        {
            ViewBag.ID = id;
            ViewBag.ListSV = await BookingDAO.GetServicesOfBill(id);
            var result = await ServiceDAO.GetAll();
            return PartialView(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddSV(BookingServices model)
        {
            try
            {
                await BookingDAO.AddSV(model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveSV(BookingServices model)
        {
            try
            {
                await BookingDAO.RemoveSV(model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Remove(int id)
        {
            var enti = await BookingDAO.GetByID(id);
            if(enti.Status != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            try
            {
                await BookingDAO.Remove(id);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Payment(int id)
        {
            var invoice = await BookingDAO.GetByID(id);
            return PartialView(invoice);
        }


        #region List Select
        private async Task<SelectList> ListRoom()
        {
            return new SelectList(await RoomDAO.GetAll(), "ID", "Name");
        }

        private async Task<MultiSelectList> ListServices()
        {
            return new SelectList(await ServiceDAO.GetAll(), "ID", "Name");
        }

        private async Task<SelectList> ListVoucher()
        {
            return new SelectList(await VoucherDAO.GetAvailable(), "Code");
        }
        #endregion
    }
}