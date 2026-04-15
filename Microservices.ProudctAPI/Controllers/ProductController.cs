using AutoMapper;
using Microservices.ProudctAPI.Data;
using Microservices.ProudctAPI.Models;
using Microservices.ProudctAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.ProudctAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private ResponseDto responseDto;
        private IMapper _mapper;
        public ProductController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> Products = _context.Products.ToList();
                responseDto.Result = _mapper.Map<IEnumerable<Product>>(Products);
            }
            catch (Exception ex)
            {
                responseDto.Success= false;
                responseDto.Message= ex.Message;
            }
            return responseDto;
        }
        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product product = _context.Products.First(x => x.ProductId == id);
                responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                responseDto.Success= false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] ProductDto productDto)
        {
            try
            {

                Product product = _mapper.Map<Product>(productDto);
                _context.Products.Add(product);
                _context.SaveChanges();
                responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                responseDto.Success = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Update([FromBody] ProductDto productDto)
        {
            try
            {

                Product product = _mapper.Map<Product>(productDto);
                _context.Products.Update(product);
                _context.SaveChanges();
                responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                responseDto.Success = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
        [HttpDelete]
        [Route("{productId:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int productId)
        {
            try
            {

                Product product = _context.Products.First(x => x.ProductId == productId);
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                responseDto.Success = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
    }
}
