using AutoMapper;
using Microservices.ProudctAPI.Models;
using Microservices.ProudctAPI.Models.Dto;

namespace Microservices.ProudctAPI
{
    public class MapConfig: Profile
    {
        public MapConfig()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
