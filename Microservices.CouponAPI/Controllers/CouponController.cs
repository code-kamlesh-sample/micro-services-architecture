using AutoMapper;
using Microservices.CouponAPI.Data;
using Microservices.CouponAPI.Models;
using Microservices.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CouponController : ControllerBase
    {
        private readonly AppDbContext _context;
        private ResponseDto responseDto;
        private IMapper _mapper;
        public CouponController(AppDbContext context, IMapper mapper)
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
                IEnumerable<Coupon> coupons = _context.Coupons.ToList();
                responseDto.Result = _mapper.Map<IEnumerable<Coupon>>(coupons);
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
                Coupon coupon = _context.Coupons.First(x => x.CouponId == id);
                responseDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                responseDto.Success= false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon = _context.Coupons.First(x => x.CouponCode.ToLower() == code.ToLower());
                responseDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                responseDto.Success = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {

                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Add(coupon);
                _context.SaveChanges();
                responseDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                responseDto.Success = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
        [HttpPut]
        public ResponseDto Update([FromBody] CouponDto couponDto)
        {
            try
            {

                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Update(coupon);
                _context.SaveChanges();
                responseDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                responseDto.Success = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
        [HttpDelete]
        [Route("{couponId:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int couponId)
        {
            try
            {

                Coupon coupon = _context.Coupons.First(x => x.CouponId == couponId);
                _context.Coupons.Remove(coupon);
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
