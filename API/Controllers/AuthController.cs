using System.Threading.Tasks;
using API.Errors;
using AutoMapper;
using Core.Dtos;
using Core.Entities.Identity;
using Core.Interfaces.services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;

        }

        /** 
            - user needs to login before making any requets
            - users were seeded into the db
        
        sample payload
        {
            "email": "gsmith@test.com",
            "password": "Pa$$w0rd"
        }
        **/
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ApiResponse(401));

            var userDto = new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user)
            };

            return Ok(userDto);
        }

        //Endpoint to create your own users
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            if (CheckEmailExistsAsync(registerUserDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] {
                        "Email address is already registered"
                    }
                });
            }

            var roleToUse = Role.User;

            var user = _mapper.Map<User>(registerUserDto);

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Password must be at least 6 characters, include a digit and at least one upper case letter"));

            //check is user is admin
            if (registerUserDto.IsAdmin)
                roleToUse = Role.Admin;


            var roleResult = await _userManager.AddToRoleAsync(user, roleToUse);

            if (!roleResult.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(new
            {
                Token = await _tokenService.CreateToken(user),
                Email = user.Email,
                Role = roleToUse
            });
        }



    }
}