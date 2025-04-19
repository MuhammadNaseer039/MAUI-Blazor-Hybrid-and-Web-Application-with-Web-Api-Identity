using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using UserManagemanetAPI.DTOs;
using UserManagemanetAPI.Services;
using UserManagementAPI.DTOs;

namespace UserManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly EmailService emailService;
        private readonly IConfiguration configuration;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, EmailService emailService, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailService = emailService;
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("register")]  
        public async Task<ActionResult<ApiResponse>> RegisterAsync([FromBody] RegisterDTO user, string role)
        {
            ApiResponse apiresponse = new();
            var userExist = await userManager.FindByEmailAsync(user.Email);

            if (userExist != null)
            {
                apiresponse.Status = "Error";
                apiresponse.Message = "User Already Exist";
                return StatusCode(StatusCodes.Status403Forbidden, apiresponse);
            }

            IdentityUser newUser = new IdentityUser()
            {
                UserName = user.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = user.Email,
            };

            if (await roleManager.RoleExistsAsync(role))
            {
                var result = await userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    var response = await userManager.AddToRoleAsync(newUser, role);
                    if (response.Succeeded)
                    {
                        await SendConfirmationEmail(user.Email, newUser);
                        apiresponse.Status = "Success";
                        apiresponse.Message = "User Created Successfully";
                        return StatusCode(StatusCodes.Status200OK,apiresponse);
                    }
                    else
                    {
                        apiresponse.Status = "Error";
                        apiresponse.Message = "Failed to create User with this Role";
                        return StatusCode(StatusCodes.Status500InternalServerError, apiresponse);
                    }

                }
                else
                {
                    apiresponse.Status = "Error";
                    apiresponse.Message = "Failed to create User with this Role";
                    return StatusCode(StatusCodes.Status500InternalServerError, apiresponse);
                }
            }
            else
            {
                apiresponse.Status = "Failed";
                apiresponse.Message = "No Role Exist";
                return StatusCode(StatusCodes.Status500InternalServerError, apiresponse);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO user)
        {
            var userexist = await userManager.FindByEmailAsync(user.Email);
            if(userexist != null)
            {
                var OkCredentials = await userManager.CheckPasswordAsync(userexist,user.Password);
                if (OkCredentials)
                {
                    var tokenhandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email,userexist.Email),
                        new Claim(ClaimTypes.NameIdentifier, userexist.Id),
                        new Claim(ClaimTypes.Name, userexist.UserName)
                    };
                    var userroles = await userManager.GetRolesAsync(userexist);
                    foreach(var role in userroles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    var tokendescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(authClaims),
                        Expires = DateTime.UtcNow.AddMinutes(3),
                        Issuer = configuration["Jwt:Issure"],
                        Audience = configuration["Jwt:Audience"],
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenhandler.CreateToken(tokendescriptor);
                    return StatusCode(StatusCodes.Status200OK,
                        new LoginApiResponse()
                        { 
                            Token = tokenhandler.WriteToken(token),
                            Expiry = (DateTime)tokendescriptor.Expires,
                            Email = userexist.Email,
                            Status = "Success",
                            Message = "User Login Successfully"

                        });
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized,
                                      new ApiResponse() { Status = "Error", Message = "Wrong password please try again" });
                }

            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new ApiResponse() { Status = "Error", Message = "No User Exist With this Email" });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string UserId, string Token)
        {
            bool isConfirm = false;
            return Redirect($"https://localhost:7059/ConfirmEmail/{isConfirm}");
        }

        private async Task SendConfirmationEmail(string email, IdentityUser user)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new
            { UserId = user.Id, Token = token }, protocol: HttpContext.Request.Scheme);

            var safeLink = HtmlEncoder.Default.Encode(confirmationLink);

            var subject = "Welcome to Dot Net Tutorials! Please Confirm Your Email";

            var messagebody = $@"
                                <div style=""font-family:Arial,Helvetica,sans-serif;font-size:16px;line-height:1.6;color:#333;"">
                                    <p>Hi {user.UserName} {user.Email},</p>
                                    <p>Thank you for creating an account at <strong>Dot Net Tutorials</strong>.
                                    To start enjoying all of our features, please confirm your email address by clicking the button below:</p>
                                    <p>
                                        <a href=""{safeLink}"" 
                                            style=""background-color:#007bff;color:#fff;padding:10px 20px;text-decoration:none;
                                              font-weight:bold;border-radius:5px;display:inline-block;"">
                                            Confirm Email
                                        </a>
                                    </p>
                                    <p>If the button doesn’t work for you, copy and paste the following URL into your browser:
                                        <br />
                                        <a href=""{safeLink}"" style=""color:#007bff;text-decoration:none;"">{safeLink}</a>
                                    </p>
                                    <p>If you did not sign up for this account, please ignore this email.</p>
                                    <p>Thanks,<br />
                                    The Dot Net Tutorials Team</p>
                                </div>
                                ";
            await emailService.SendEmailAsync(email, subject, messagebody, true);
        }
    }
}
