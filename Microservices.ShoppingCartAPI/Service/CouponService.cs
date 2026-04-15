using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Microservices.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> GetCoupons(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resDto!=null && resDto.Success)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resDto.Result));
            }
            return new CouponDto();
        }
    }
}
