using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
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

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, EmailService emailService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("register")]  
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO user, string role)
        {
            var userExist = await userManager.FindByEmailAsync(user.Email);

            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse() { Status = "Error", Message = "User Already Exist" });
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
                        return StatusCode(StatusCodes.Status200OK, new ApiResponse()
                        { Status = "Success", Message = "User Created Successfully" });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse()
                        { Status = "Error", Message = "Failed to create User with this Role" });
                    }

                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse()
                    { Status = "Error", Message = "Failed to create User with this Role" });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new ApiResponse() { Status = "Failed", Message = "No Role Exist" });
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
