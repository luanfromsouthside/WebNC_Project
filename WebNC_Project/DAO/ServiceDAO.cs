using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public class ServiceDAO
    {
        private static readonly ResortContext db = new ResortContext();
        private static ServiceDAO instance;
        private ServiceDAO() { }
        public static ServiceDAO Instance
        {
            get
            {
                if (instance == null) instance = new ServiceDAO();
                return instance;
            }
        }
        public IEnumerable<Service> GetAll()
        {
            return db.Services.ToList();
        }

        public Service GetByID(string id)
        {
            return db.Services.Find(id);
        }

        public int Create(Service service)
        {
            service.ID = service.ID.Trim().ToUpper();
            service.Description = service.Description.Trim();
            db.Services.Add(service);
            return db.SaveChanges();
        }

        public int Remove(string id)
        {
            Service service = GetByID(id);
            db.Services.Remove(service);
            return db.SaveChanges();
        }

        public int Edit(Service service)
        {
            Service enti = db.Services.SingleOrDefault(s => s.ID == service.ID);
            if (enti != null)
            {
                enti.Name = service.Name;
                enti.Description = service.Description;
                enti.Price = service.Price;
                return db.SaveChanges();
            }
            throw new Exception("Entity does not exist");
        }
    }
}