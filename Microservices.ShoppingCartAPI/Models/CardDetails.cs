using Microservices.ShoppingCartAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservices.ShoppingCartAPI.Models
{
    public class CardDetails
    {
        [Key]
        public int CardDetailId { get; set; }

        public int CardHeaderId { get; set; }
        [ForeignKey("CardHeaderId")]
        public CardHeader CardHeader { get; set; }

        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}
