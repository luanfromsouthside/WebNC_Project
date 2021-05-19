using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public class RoomTypeDAO
    {
        private static readonly ResortContext db = new ResortContext();
        private static RoomTypeDAO instance;
        private RoomTypeDAO() { }
        public static RoomTypeDAO Instance
        {
            get
            {
                if (instance == null) instance = new RoomTypeDAO();
                return instance;
            }
        }
        public async Task<IEnumerable<RoomType>> GetAll()
        {
            return await db.RoomTypes.ToListAsync();
        }

        public async Task<RoomType> GetByID(string id)
        {
            return await db.RoomTypes.FindAsync(id);
        }

        public int Create(RoomType type)
        {
            type.ID = type.ID.Trim().ToUpper();
            db.RoomTypes.Add(type);
            return db.SaveChanges();
        }

        public async Task<int> Remove(string id)
        {
            RoomType type = await GetByID(id);
            db.RoomTypes.Remove(type);
            return db.SaveChanges();
        }

        public int Edit(RoomType type)
        {
            RoomType enti = db.RoomTypes.SingleOrDefault(s => s.ID.ToLower() == type.ID.Trim().ToLower());
            if (enti != null)
            {
                enti.NameType = type.NameType;
                return db.SaveChanges();
            }
            throw new Exception("Entity does not exist");
        }
    }
}