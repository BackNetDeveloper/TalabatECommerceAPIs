using APIsMainProject.Dtos;
using APIsMainProject.ResponseModule;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Security.Claims;

namespace APIsMainProject.Controllers
{
   
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(Email);
            if (user is null)
                return NotFound(new ApiResponse(404));
            return new UserDto()
            {
                Email = Email,
                DisplayName = user.DisplayName,
                Token = tokenService.CreateToken(user)
            };
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var Result = await signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);
            if(!Result.Succeeded)
                return Unauthorized(new ApiResponse(401));

            return new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = tokenService.CreateToken(user)
            };
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] {"Email Address Is In Use!!" },
                    Details = "You Email Is Already Registered => Login Now !"
                    
                });
            }
            var user = new AppUser 
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplyName,
                UserName = registerDto.Email.Split("@")[0]
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));
            return new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = tokenService.CreateToken(user)
            };
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync(string Email)
        => await userManager.FindByEmailAsync(Email) != null;

        [Authorize]
        [HttpGet("GetCurrentUserAddress")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(x => x.Address)
                                              .FirstOrDefaultAsync(x=>x.Email==Email);
            var MappedAddress = mapper.Map<AddressDto>(user.Address);
            return Ok(MappedAddress);
        }
        [Authorize]
        [HttpPost("UpdateCurrentUserAddress")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(x => x.Address)
                                              .FirstOrDefaultAsync(x => x.Email == Email);
            user.Address  = mapper.Map<Address>(addressDto);
            var result = await userManager.UpdateAsync(user);
            if(result.Succeeded)
                return Ok(mapper.Map<AddressDto>(user.Address));
            return BadRequest(new ApiResponse(400,"Problem Updating The User Address"));

        }
        }
}
