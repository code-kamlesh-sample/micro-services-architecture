namespace Microservices.ShoppingCartAPI.Models.Dto
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        public string? CouponCode { get; set; }
        public double DiscoundAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
