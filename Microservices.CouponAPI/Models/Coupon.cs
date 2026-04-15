using System.ComponentModel.DataAnnotations;

namespace Microservices.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public double DiscoundAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
