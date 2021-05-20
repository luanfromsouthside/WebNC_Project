using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public static class RoomDAO
    {
        public static async Task<IEnumerable<Room>> GetAll()
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Rooms.Include(r => r.RoomType).Include(r => r.Images).ToListAsync();
            }
        }

        public static async Task<Room> GetByID(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.Rooms.Include(r => r.RoomType).Include(r => r.Images).SingleOrDefaultAsync(r => r.ID == id);
            }
        }

        public static async Task<int> Create(Room room)
        {
            using (ResortContext db = new ResortContext())
            {
                room.ID = room.ID.Trim().ToUpper();
                db.Rooms.Add(room);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Remove(string id)
        {
            using (ResortContext db = new ResortContext())
            {
                Room room = await db.Rooms.Include(r => r.Images).SingleOrDefaultAsync(r => r.ID == id);
                foreach(var img in room.Images.ToList())
                {
                    db.Images.Remove(img);
                }
                db.Rooms.Remove(room);
                return db.SaveChanges();
            }
        }

        public static async Task<int> EditInfo(Room room)
        {
            using (ResortContext db = new ResortContext())
            {
                Room enti = await db.Rooms.SingleOrDefaultAsync(s => s.ID.ToLower() == room.ID.Trim().ToLower());
                if (enti != null)
                {
                    enti.Name = room.Name;
                    enti.Price = room.Price;
                    enti.Description = room.Description;
                    enti.TypeID = room.TypeID;
                    enti.Status = room.Status;
                    enti.Adult = room.Adult;
                    enti.Child = room.Child;
                    return await db.SaveChangesAsync();
                }
                throw new Exception("Entity does not exist");
            }
        }
    }
}