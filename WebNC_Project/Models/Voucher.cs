namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Voucher
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Voucher()
        {
            Bookings = new HashSet<Booking>();
        }

        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        public int Discount { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ToDate { get; set; }

        public int Condition { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
