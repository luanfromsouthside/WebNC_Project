using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public class PermissionDAO
    {
        private static readonly ResortContext db = new ResortContext();
        private static PermissionDAO instance;
        private PermissionDAO() { }
        public static PermissionDAO Instance
        {
            get
            {
                if (instance == null) instance = new PermissionDAO();
                return instance;
            }
        }

        public IEnumerable<Permission> GetAll()
        {
            return db.Permissions.ToList();
        }
    }
}