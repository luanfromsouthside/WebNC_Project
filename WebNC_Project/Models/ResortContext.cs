using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebNC_Project.Models
{
    public partial class ResortContext : DbContext
    {
        public ResortContext()
            : base("name=ResortConnection")
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<Rule> Rules { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<Supply> Supplies { get; set; }
        public virtual DbSet<SuppliesForRoom> SuppliesForRooms { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasMany(e => e.Services)
                .WithMany(e => e.Bookings)
                .Map(m => m.ToTable("BookingServices").MapLeftKey("BookingID").MapRightKey("ServiceID"));

            modelBuilder.Entity<RoomType>()
                .HasMany(e => e.Rooms)
                .WithOptional(e => e.RoomType)
                .HasForeignKey(e => e.TypeID);
        }
    }
}
