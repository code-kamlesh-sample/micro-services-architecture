using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Microservices.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartBaseOnLoggedUser());
        }

        public async Task<IActionResult> Remove(int cardDetailsId)
        {
            var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var response = await _cartService.RemoveFromCartAsync(cardDetailsId);
            if (response != null && response.Result != null)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));   
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CardDto cardDto)
        {
            
            ResponseDto? response = await _cartService.ApplyCouponAsync(cardDto);
            if (response != null && response.Success)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CardDto cardDto)
        {
            cardDto.CardHeader.CouponCode= string.Empty;
            ResponseDto? response = await _cartService.ApplyCouponAsync(cardDto);
            if (response != null && response.Success)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        private async Task<CardDto?> LoadCartBaseOnLoggedUser()
        {
            var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;  
            var response = await _cartService.GetCartByUserIdAsync(userId);
            if (response != null && response.Result != null)
            {
                CardDto cardDto = JsonConvert.DeserializeObject<CardDto>(Convert.ToString(response.Result));
                return cardDto;
            }

            return new CardDto();
        }
    }
}
