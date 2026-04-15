using Microservices.AuthAPI.Models.Dto;
using Microservices.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private ResponseDto _response;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var errorMessage = await _authService.Register(registrationRequestDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.Success = false;
                _response.Message= errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _authService.Login(loginRequestDto);
            if(loginResponse.User == null)
            {
                _response.Success = false;
                _response.Message = "UserName and Password Incorrect.";
                return BadRequest(_response);
            }
            _response.Result= loginResponse;
            return Ok(_response);
        }
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var assignRoleSuccefull = await _authService.AssignRole(registrationRequestDto.Email,registrationRequestDto.Role.ToUpper());
            if (!assignRoleSuccefull)
            {
                _response.Success = false;
                _response.Message = "Error Encountered.";
                return BadRequest(_response);
            }
            _response.Result = assignRoleSuccefull;
            return Ok(_response);
        }
    }
}
