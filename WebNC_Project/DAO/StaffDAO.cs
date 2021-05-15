using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public class StaffDAO
    {
        private static readonly ResortContext db = new ResortContext();
        private static StaffDAO instance;
        private StaffDAO() { }
        public static StaffDAO Instance
        {
            get
            {
                if (instance == null) instance = new StaffDAO();
                return instance;
            }
        }
        public IEnumerable<Staff> GetAll()
        {
            return db.Staffs.ToList();
        }

        public Staff GetByID(string id)
        {
            return db.Staffs.Find(id);
        }

        public int Create(Staff staff)
        {
            db.Staffs.Add(staff);
            return db.SaveChanges();
        }

        public int Remove(string id)
        {
            Staff staff = GetByID(id);
            db.Staffs.Remove(staff);
            return db.SaveChanges();
        }

        public int Edit(Staff staff)
        {
            Staff enti = db.Staffs.SingleOrDefault(s => s.ID == staff.ID);
            if (enti != null)
            {
                enti.Name = staff.Name;
                enti.Birth = staff.Birth;
                enti.Gender = staff.Gender;
                enti.Password = staff.Password;
                enti.PermissionID = staff.PermissionID;
                enti.Phone = staff.Phone;
                enti.Email = staff.Email;
                return db.SaveChanges();
            }
            throw new Exception("Entity does not exist");
        }
    }
}