namespace Microservices.Web.Models
{
    public class CardDetailsDto
    {
        public int CardDetailId { get; set; }
        public int CardHeaderId { get; set; }        
        public CardHeaderDto? CardHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
    }
}
