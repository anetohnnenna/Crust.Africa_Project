using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using UserAuthentication.Entities;
using UserAuthentication.Interface;
using UserAuthentication.Models;
using UserAuthentication.Services;

namespace UserAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : Controller
    {
    
        public readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthenticateController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }


        [Route("Registration")]
        [HttpPost]
        public async Task<IActionResult> Registration([FromBody] RegistrationModel reg)
        {
            
            if (ModelState.IsValid)
            {
                GenericApiResponse<RegistrationTable> result = new GenericApiResponse<RegistrationTable>();
                var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

                var regex = new Regex(pattern);
                bool email = regex.IsMatch(reg.Email);
                if (email)
                {
                    RegistrationTable request = new RegistrationTable();
                    request.UserName = reg.UserName;
                    request.Email = reg.Email;
                    request.Password = reg.Password;
                    result = await _userService.UserReg(request);
                    return Ok(result);
                }
                else
                {
                    result.ResponseCode = ResponseCodes.Failure;
                    result.ResponseDescription = "Kindly input your correct email address";
                    return Ok(result);
                }

               
            }
            else
            {
                return BadRequest("Kindly input your details");

            }

        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel reg)
        {
            if (ModelState.IsValid)
            {
                LoginTable login = new LoginTable();
                RegistrationTable request = new RegistrationTable();
                request.Email = reg.Email;
                request.Password = reg.Password;
                GenericApiResponse<RegistrationTable> result = await _userService.LoginUser(request);
                if (result.ResponseCode == "00")
                {
                    var authClaims = new List<Claim>
                {
                        new Claim(ClaimTypes.Name, reg.Email),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };
                    var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Secret")));
                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudence"],
                        expires: DateTime.Now.AddHours(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                        );
                    if (token != null)
                    {
                        login.LoginMethod = "JWT Token";
                        login.Email = reg.Email;
                        login.LoginTime = DateTime.Now;
                        login.LoginExpireTime = DateTime.Now.AddHours(1);
                        login.Status = "Login Successfully";
                        login.StatusCode = 1;
                        await _userService.LoginDetails(login);
                    }
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = DateTime.Now.AddHours(1),
                    });
                    //return Ok(result);
                }
                return Ok(result);
            }
            else
            {
                return BadRequest("Kindly input your details");

            }

        }
     

        [Route("GoogleLogin")]
        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var propertites = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(propertites, GoogleDefaults.AuthenticationScheme);
        }

        [Route("GoogleResponse")]
        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new 
                { 
                   claim.Issuer,
                   claim.OriginalIssuer,
                   claim.Type,
                   claim.Value
                });
            if(claims != null)
            {
                LoginTable login = new LoginTable();
                login.LoginMethod = claims.Select(a => a.Issuer).FirstOrDefault();
                login.Email = claims.Select(a => a.Value).LastOrDefault();
                login.LoginTime = DateTime.Now;
                login.LoginExpireTime = DateTime.Now.AddHours(1);
                login.Status = "Login Successfully";
                login.StatusCode = 1;
                await _userService.LoginDetails(login);
            }
            
            return Json(claims);
        }

    }
}
