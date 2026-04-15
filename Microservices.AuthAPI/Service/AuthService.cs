using Microservices.AuthAPI.Data;
using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Models.Dto;
using Microservices.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Microservices.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if(!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginReponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x=>x.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            
            bool isValid = await _userManager.CheckPasswordAsync(user,loginRequestDto.Password);
            if (user == null || isValid == false)
            {
                return new LoginReponseDto();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user,roles);
            UserDto userDto = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
            LoginReponseDto loginReponseDto = new LoginReponseDto()
            {
                User = userDto,
                Token = token
            };
            return loginReponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email= registrationRequestDto.Email,
                NormalizedEmail=registrationRequestDto.Email.ToUpper(),
                Name= registrationRequestDto.Name,
                PhoneNumber= registrationRequestDto.PhoneNumber
            };

            try
            {
                var result =await _userManager.CreateAsync(user,registrationRequestDto.Password);
                if(result.Succeeded)
                {
                    var userDetail = _context.ApplicationUsers.First(x => x.UserName == registrationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email= userDetail.Email,
                        Id = userDetail.Id,
                        Name= userDetail.Name,
                        PhoneNumber = userDetail.PhoneNumber
                    };
                    return string.Empty;
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
