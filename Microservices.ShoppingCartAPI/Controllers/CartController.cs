using AutoMapper;
using Microservices.ShoppingCartAPI.Data;
using Microservices.ShoppingCartAPI.Models;
using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Microservices.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;
        private ResponseDto responseDto;
        private IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;

        public CartController(AppDbContext context, IMapper mapper, IProductService productService, ICouponService couponService)
        {
            _context = context;
            _mapper = mapper;
            responseDto = new ResponseDto();
            _productService = productService;
            _couponService = couponService;
        }
        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody]CardDto cardDto)
        {
            try
            {
                var cartFromDb = await _context.CardHeader.FirstOrDefaultAsync(x => x.UserId == cardDto.CardHeader.UserId);
                cartFromDb.CouponCode = cardDto.CardHeader.CouponCode;
                _context.CardHeader.Update(cartFromDb);
                await _context.SaveChangesAsync();
                responseDto.Result = true;
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message.ToString();
                responseDto.Success = false;
            }
            return responseDto;
        }
        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] CardDto cardDto)
        {
            try
            {
                var cartFromDb = await _context.CardHeader.FirstOrDefaultAsync(x => x.UserId == cardDto.CardHeader.UserId);
                cartFromDb.CouponCode = string.Empty;
                _context.CardHeader.Update(cartFromDb);
                await _context.SaveChangesAsync();
                responseDto.Result = true;
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message.ToString();
                responseDto.Success = false;
            }
            return responseDto;
        }

        [HttpPost("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CardDto cardDto = new()
                {
                    CardHeader = _mapper.Map<CardHeaderDto>(_context.CardHeader.First(x=>x.UserId == userId))
                };
                
                cardDto.CardDetails = _mapper.Map<IEnumerable<CardDetailsDto>>(_context.CardDetails.Where(x => x.CardHeaderId == cardDto.CardHeader.CardHeaderId));

                IEnumerable<ProductDto> productsData = await _productService.GetProducts();
                foreach (var item in cardDto.CardDetails)
                {
                    item.Product = productsData.FirstOrDefault(x => x.ProductId == item.ProductId);
                    cardDto.CardHeader.CardTotal += (item.Count * item.Product.Price);
                }
                
                // apply coupon if any
                if(!string.IsNullOrEmpty(cardDto.CardHeader.CouponCode))
                {
                    CouponDto couponDto = await _couponService.GetCoupons(cardDto.CardHeader.CouponCode);
                    if(couponDto != null && cardDto.CardHeader.CardTotal > couponDto.MinAmount)
                    {
                        cardDto.CardHeader.CardTotal -= couponDto.DiscoundAmount;
                        cardDto.CardHeader.Discount = couponDto.DiscoundAmount;
                    }
                }

                responseDto.Result = cardDto;
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message.ToString();
                responseDto.Success = false;
            }
            return responseDto;
        }
        
        [HttpPost("CardUpsert")]
        public async Task<ResponseDto> CardUpsert(CardDto cardDto)
        {
            try
            {
                var cardHeaderFromDb = await _context.CardHeader.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cardDto.CardHeader.UserId);
                if (cardHeaderFromDb == null)
                {
                    //create header and details
                    CardHeader cardHeader = _mapper.Map<CardHeader>(cardDto.CardHeader);
                    _context.Add(cardHeader);
                    await _context.SaveChangesAsync();
                    cardDto.CardDetails.First().CardHeaderId = cardHeader.CardHeaderId;
                    _context.CardDetails.Add(_mapper.Map<CardDetails>(cardDto.CardDetails.First()));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cardDetailsFromDb = await _context.CardDetails.AsNoTracking()
                        .FirstOrDefaultAsync(x=>x.ProductId == cardDto.CardDetails.First().ProductId && x.CardHeaderId == cardHeaderFromDb.CardHeaderId);
                    if(cardDetailsFromDb == null)
                    {
                        //create card details
                        cardDto.CardDetails.First().CardHeaderId = cardHeaderFromDb.CardHeaderId;
                        _context.CardDetails.Add(_mapper.Map<CardDetails>(cardDto.CardDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in card details
                        cardDto.CardDetails.First().Count += cardDetailsFromDb.Count;
                        cardDto.CardDetails.First().CardHeaderId = cardDetailsFromDb.CardHeaderId;
                        cardDto.CardDetails.First().CardDetailId = cardDetailsFromDb.CardDetailId;
                        _context.CardDetails.Update(_mapper.Map<CardDetails>(cardDto.CardDetails.First())) ;
                        await _context.SaveChangesAsync();
                    }
                }
                responseDto.Result= cardDto;
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message.ToString();
                responseDto.Success= false;
            }
            return responseDto;
        }
        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cardDetailsId)
        {
            try
            {
                CardDetails cardDetails= _context.CardDetails.First(x => x.CardDetailId== cardDetailsId);
                int totalCountCartItem = _context.CardDetails.Where(x => x.CardHeaderId == cardDetails.CardHeaderId).Count();
                _context.CardDetails.Remove(cardDetails);

                if (totalCountCartItem == 1)
                {
                    var cardHeaderToRemove = await _context.CardHeader.FirstOrDefaultAsync(x => x.CardHeaderId == cardDetails.CardHeaderId);
                    _context.CardHeader.Remove(cardHeaderToRemove);
                }
                await _context.SaveChangesAsync();
                responseDto.Result = true;
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message.ToString();
                responseDto.Success = false;
            }
            return responseDto;
        }

    }
}
