using AutoMapper;
using Microservices.CouponAPI.Models;
using Microservices.CouponAPI.Models.Dto;

namespace Microservices.CouponAPI
{
    public class MapConfig: Profile
    {
        public MapConfig()
        {
            CreateMap<Coupon, CouponDto>().ReverseMap();
        }
    }
}
