using WebAPI.Models.Authentication;
using WebAPI.Models.Authentication.Jwt;
using WebAPI.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]API")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings jwtSettings;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, JwtSettings jwtSettings)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.jwtSettings = jwtSettings;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            APIResponse apiResponse = new APIResponse();
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = new UserTokens();
                token = Helpers.JwtHelpers.JwtHelpers.GenTokenkey(new UserTokens()
                {
                    Email = user.Email,
                    Mobile = user.Mobile,
                    Company = user.Company,
                    GuidId = Guid.NewGuid(),
                    UserName = user.FullName,
                    Id = Guid.NewGuid()
                }, jwtSettings);

                apiResponse.message = "Logged in successfully";
                apiResponse.data = token;
                return Ok(apiResponse);
            }

            apiResponse.message = "Invalid email or password";
            apiResponse.data = null;
            return Ok(apiResponse);
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                FullName = model.Username,
                Mobile = model.Mobile,
                Company = model.Company
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
    }
}
