using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIProject.Dto;
using WebAPIProject.Models;

namespace WebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }
        //create User
        [HttpPost("register")]//api/account/register
        public async Task<IActionResult> RegisterAsync(RegisterUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email
                };
                //create account
                IdentityResult result=
                    await userManager.CreateAsync(user, userDTO.Password);
                if (result.Succeeded)
                {
                    return Ok("Account Create Success");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("AccountErrors", item.Description);
                }
            }
            return BadRequest(ModelState);
        }

        //Login Create Token //api/account/Login
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                //check 
               ApplicationUser user=await userManager.FindByNameAsync(userDto.UserName);
                if (user!=null)
                {
                    bool found=await userManager.CheckPasswordAsync(user, userDto.Password);
                    if (found)
                    {
                        #region Create toke
                        string jit = Guid.NewGuid().ToString();

                        var roles = await userManager.GetRolesAsync(user);
                        
                        List<Claim> myClaims = new List<Claim>();
                        myClaims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id));
                        myClaims.Add(new Claim(ClaimTypes.Name,user.UserName));
                        myClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, jit));

                        if (roles != null)
                        {
                            foreach (var role in roles)
                            {
                                myClaims.Add(new Claim(ClaimTypes.Role, role));
                            }
                        }

                        SymmetricSecurityKey signInKey = 
                            new(Encoding.UTF8.GetBytes(config["JWT:Key"]));

                        SigningCredentials signInCred = 
                            new(signInKey, SecurityAlgorithms.HmacSha256);


                        JwtSecurityToken mytoken =
                            new (
                                issuer: config["JWT:Iss"],
                                audience: config["JWT:Aud"],
                                claims:myClaims,
                                expires:DateTime.Now.AddHours(1),
                                signingCredentials: signInCred);
                        #endregion 

                        return Ok(new
                        {
                            exp=DateTime.Now.AddHours(1),
                            token=new JwtSecurityTokenHandler().WriteToken(mytoken)
                        });
                    }
                }
                ModelState.AddModelError("error", "Invalid Account");
            }
            return BadRequest(ModelState);
        }
    }
}
