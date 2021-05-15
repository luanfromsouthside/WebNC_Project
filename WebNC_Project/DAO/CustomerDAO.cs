using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public class CustomerDAO
    {
        private static readonly ResortContext db = new ResortContext();
        private static CustomerDAO instance;
        private CustomerDAO() { }
        public static CustomerDAO Instance
        {
            get
            {
                if (instance == null) instance = new CustomerDAO();
                return instance;
            }
        }
        public IEnumerable<Customer> GetAll()
        {
            return db.Customers.ToList();
        }

        public Customer GetByID(string id)
        {
            return db.Customers.Find(id);
        }

        public int Create(Customer customer)
        {
            db.Customers.Add(customer);
            return db.SaveChanges();
        }

        public int Remove(string id)
        {
            Customer customer = GetByID(id);
            db.Customers.Remove(customer);
            return db.SaveChanges();
        }

        public int Edit(Customer customer)
        {
            Customer enti = db.Customers.SingleOrDefault(s => s.ID == customer.ID);
            if (enti != null)
            {
                enti.Name = customer.Name;
                enti.Birth = customer.Birth;
                enti.Gender = customer.Gender;
                enti.Password = customer.Password;
                enti.Phone = customer.Phone;
                return db.SaveChanges();
            }
            throw new Exception("Entity does not exist");
        }
    }
}