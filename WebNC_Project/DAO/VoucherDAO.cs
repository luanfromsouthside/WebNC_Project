using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public class VoucherDAO
    {
        private static readonly ResortContext db = new ResortContext();
        private static VoucherDAO instance;
        private VoucherDAO() { }
        public static VoucherDAO Instance
        {
            get
            {
                if (instance == null) instance = new VoucherDAO();
                return instance;
            }
        }
        public IEnumerable<Voucher> GetAll()
        {
            return db.Vouchers.ToList();
        }

        public Voucher GetByID(string id)
        {
            return db.Vouchers.Find(id);
        }

        public int Create(Voucher voucher)
        {
            voucher.Code = voucher.Code.Trim().ToUpper();
            db.Vouchers.Add(voucher);
            return db.SaveChanges();
        }

        public int Remove(string id)
        {
            Voucher voucher = GetByID(id);
            db.Vouchers.Remove(voucher);
            return db.SaveChanges();
        }

        public int Edit(Voucher voucher)
        {
            Voucher enti = db.Vouchers.SingleOrDefault(s => s.Code.ToLower() == voucher.Code.Trim().ToLower());
            if (enti != null)
            {
                enti.Condition = voucher.Condition;
                enti.Discount = voucher.Discount;
                enti.FromDate = voucher.FromDate;
                enti.ToDate = voucher.ToDate;
                return db.SaveChanges();
            }
            throw new Exception("Entity does not exist");
        }
    }
}