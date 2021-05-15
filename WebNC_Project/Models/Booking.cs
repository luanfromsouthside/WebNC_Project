namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            Services = new HashSet<Service>();
        }

        [StringLength(10)]
        public string ID { get; set; }

        [StringLength(20)]
        public string CustomerID { get; set; }

        [StringLength(20)]
        public string StaffID { get; set; }

        [Required]
        [StringLength(10)]
        public string RoomID { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CheckinDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CheckoutDate { get; set; }

        public string Status { get; set; }

        public int Adult { get; set; }

        public int Child { get; set; }

        [StringLength(20)]
        public string VoucherCode { get; set; }

        public string FeedBack { get; set; }

        public int? Rate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Room Room { get; set; }

        public virtual Staff Staff { get; set; }

        public virtual Voucher Voucher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service> Services { get; set; }
    }
}
