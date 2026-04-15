using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Microservices.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if(resDto.Success)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resDto.Result));
            }
            return new List<ProductDto>();
        }
    }
}
