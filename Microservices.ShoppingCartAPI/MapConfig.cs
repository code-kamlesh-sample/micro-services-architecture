using AutoMapper;
using Microservices.ShoppingCartAPI.Models;
using Microservices.ShoppingCartAPI.Models.Dto;

namespace Microservices.ShoppingCartAPI
{
    public class MapConfig: Profile
    {
        public MapConfig()
        {
            CreateMap<CardDetails, CardDetailsDto>().ReverseMap();
            CreateMap<CardHeader, CardHeaderDto>().ReverseMap();
        }
    }
}
