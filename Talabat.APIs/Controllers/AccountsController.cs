using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiErrorResponse(401));
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            return result.Succeeded ?
                new UserDto()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await tokenService.CreateTokenAsync(user, userManager)
                }
                : Unauthorized(new ApiErrorResponse(401));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new APiValidationErrorResponse() { Errors = new string[] { "This Email is Already Exists" } });
            var user = new AppUser()
            {
                DisplayName = model.DiplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };
            var createdUser = await userManager.CreateAsync(user, model.Password);
            return createdUser.Succeeded ?
                new UserDto()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await tokenService.CreateTokenAsync(user, userManager)
                }
                : Unauthorized(new ApiErrorResponse(401));
        }

        [HttpGet("currentuser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user, userManager)

            });
        }

        [HttpGet("address"), Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await userManager.FindUserWithAddressByEmailAsync(User);
            var address = mapper.Map<Address, AddressDto>(user.Address);
            return Ok(address);
        }

        [Authorize, HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
        {
            var address = mapper.Map<AddressDto, Address>(updatedAddress);
            var user = await userManager.FindUserWithAddressByEmailAsync(User);
            address.Id = user.Address.Id;
            user.Address = address;
            var result = await userManager.UpdateAsync(user);
            return result.Succeeded ? Ok(updatedAddress) : BadRequest(new ApiErrorResponse(400));
        }

        [HttpGet("checkemail")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;
        }
    }
}