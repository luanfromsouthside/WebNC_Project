namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            BookingServices = new HashSet<BookingServices>();
        }
        [Display(Name = "Mã hóa đơn")]
        public int ID { get; set; }

        [MaxLength(20,ErrorMessage ="{0} contains at most 20 characters")]
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Khách hàng")]
        public string CustomerID { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(10,ErrorMessage ="{0} contains at most 10 characters")]
        [Display(Name = "Phòng")]
        public string RoomID { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Ngày Check in")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [ValidationCheckIn]
        public DateTime CheckinDate { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Ngày Check out")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CheckoutDate { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; }

        [Range(1,int.MaxValue,ErrorMessage = "{0} is not valid")]
        [Display(Name = "Người lớn")]
        [Required(ErrorMessage = "{0} is required")]
        public int Adult { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "{0} is not valid")]
        [Display(Name = "Trẻ em")]
        public int Child { get; set; }

        [MaxLength(20, ErrorMessage = "{0} contains at most 20 characters")]
        [Display(Name = "Mã voucher")]
        public string VoucherCode { get; set; }

        public string FeedBack { get; set; }

        public int? Rate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Room Room { get; set; }

        public virtual Voucher Voucher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingServices> BookingServices { get; set; }

        [NotMapped]
        public string GetStatus
        {
            get
            {
                switch (Status) {
                    case "checkin": return "Check In";
                    case "checkout": return "Check Out";
                    case "confirm": return "Đã xác nhận";
                    case "cancel": return "Cancel";
                    case "payment": return "Đã thanh toán";
                    default: return "Chờ xác nhận";
                }
            }
        }

        [NotMapped] 
        public string GetColor
        {
            get
            {
                switch (Status)
                {
                    case "checkin": return "warning";
                    case "checkout": return "success";
                    case "confirm": return "success";
                    case "cancel": return "danger";
                    case "payment": return "success";
                    default: return "secondary";
                }
            }
        }

        public static double GetPrice(Booking booking)
        {
            double price = booking.Room.Price * booking.CheckoutDate.Date.Subtract(booking.CheckinDate.Date).Days;
            if (booking.BookingServices.Count() > 0)
            {
                foreach (var sv in booking.BookingServices)
                {
                    price += sv.Service.Price;
                }
            }
            if(booking.VoucherCode != null)
            {
                price = price * ((100 - booking.Voucher.Discount) / 100.0);
            }
            return price;
        }
    }

    public class ValidationCheckIn:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = ((DateTime)value).ToLocalTime();
            DateTime today = DateTime.Now;
            //if(DateTime.Compare(date.Date, today.Date)<0 || DateTime.Compare(date.Date,today.AddMonths(1).Date)>0)
            if(date.Date < today.Date || date.Date > today.AddMonths(1).Date)
            {
                return new ValidationResult($"Checkin Date must be beetwen {DateTime.Now.AddDays(1).ToString("dd/MM/yyyy")} to {DateTime.Now.AddDays(1).AddMonths(1).ToString("dd/MM/yyyy")}");
            }
            return ValidationResult.Success;
        }
    }

}
