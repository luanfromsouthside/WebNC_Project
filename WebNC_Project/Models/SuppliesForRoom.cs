namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SuppliesForRoom
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string RoomID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string SupplyID { get; set; }

        public int Count { get; set; }

        public virtual Room Room { get; set; }

        public virtual Supply Supply { get; set; }
    }
}
