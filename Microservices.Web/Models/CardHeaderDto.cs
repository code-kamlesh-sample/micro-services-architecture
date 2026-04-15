namespace Microservices.Web.Models
{
    public class CardHeaderDto
    {
        public int CardHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double CardTotal { get; set; }
    }
}
