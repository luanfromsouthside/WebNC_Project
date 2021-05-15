namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Image
    {
        [Key]
        [StringLength(450)]
        public string URL { get; set; }

        [StringLength(10)]
        public string RoomID { get; set; }

        public virtual Room Room { get; set; }
    }
}
