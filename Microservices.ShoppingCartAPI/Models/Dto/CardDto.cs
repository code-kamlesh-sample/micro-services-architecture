namespace Microservices.ShoppingCartAPI.Models.Dto
{
    public class CardDto
    {
        public CardHeaderDto CardHeader { get; set; }
        public IEnumerable<CardDetailsDto>?  CardDetails { get; set; }
    }
}
