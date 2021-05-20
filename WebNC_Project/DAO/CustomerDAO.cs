using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public static class CustomerDAO
    {
        public static async Task<IEnumerable<Customer>> GetAll()
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Customers.ToListAsync();
            }
        }

        public static async Task<Customer> GetByID(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.Customers.FindAsync(id);
            }
            
        }

        public static async Task<int> Create(Customer customer)
        {
            using (ResortContext db = new ResortContext())
            {
                db.Customers.Add(customer);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Remove(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                Customer customer = await db.Customers.FindAsync(id);
                db.Customers.Remove(customer);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Edit(Customer customer)
        {
            using (ResortContext db = new ResortContext())
            {
                Customer enti = await db.Customers.FindAsync(customer.ID);
                if (enti != null)
                {
                    enti.Name = customer.Name;
                    enti.Birth = customer.Birth;
                    enti.Gender = customer.Gender;
                    enti.Password = customer.Password;
                    enti.Phone = customer.Phone;
                    return await db.SaveChangesAsync();
                }
                throw new Exception("Entity does not exist");
            }
        }
    }
}