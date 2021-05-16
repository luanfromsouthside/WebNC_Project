using System;
using System.Collections.Generic;
using System.Linq;
using WebNC_Project.Models;
using System.Web;

namespace WebNC_Project.DAO
{
    public class SupplyDAO
    {
        private static readonly ResortContext db = new ResortContext();
        private static SupplyDAO instance;
        private SupplyDAO() { }
        public static SupplyDAO Instance
        {
            get
            {
                if (instance == null) instance = new SupplyDAO();
                return instance;
            }
        }
        public IEnumerable<Supply> GetAll()
        {
            return db.Supplies.ToList();
        }

        public Supply GetByID(string id)
        {
            return db.Supplies.Find(id);
        }

        public int Create(Supply sup)
        {
            sup.ID = sup.ID.Trim().ToUpper();
            db.Supplies.Add(sup);
            return db.SaveChanges();
        }

        public int Remove(string id)
        {
            Supply sup = GetByID(id);
            db.Supplies.Remove(sup);
            return db.SaveChanges();
        }

        public int Edit(Supply sup, string editType, int? count)
        {
            Supply enti = db.Supplies.SingleOrDefault(s => s.ID == sup.ID);
            if (enti != null)
            {
                switch (editType)
                {
                    case "newcount":
                        enti.Total = (int)count;
                        foreach (var item in db.SuppliesForRooms) db.SuppliesForRooms.Remove(item);
                        break;
                    case "addcount":
                        enti.Total += (int)count;
                        break;
                    default: break;
                }
                enti.Name = sup.Name;
                return db.SaveChanges();
            }
            throw new Exception("Entity does not exist");
        }
    }
}