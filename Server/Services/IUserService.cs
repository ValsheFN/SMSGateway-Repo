using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SMSGateway.Server.Infrastructure;
using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System;
using System.Web;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using FluentEmail.Core;
using Newtonsoft.Json.Linq;
using System.Security.Policy;

namespace SMSGateway.Server.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
        Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);
        Task<UserManagerResponse> ForgetPasswordAsync(string email);
        Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public readonly AuthOptions _authOptions;
        private readonly IMailService _mailService;

        public UserService(UserManager<ApplicationUser> userManager,
                            IConfiguration configuration,
                            AuthOptions authOptions,
                            IMailService mailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _authOptions = authOptions;
            _mailService = mailService;
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Some data is missing");
            }

            var userEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userEmail != null)
            {
                return new UserManagerResponse
                {
                    Message = "User is already exists",
                    IsSuccess = false

                };
            }

            var ApplicationUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var passwordError = "";
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            if (!(model.Password.Any(char.IsUpper)))
            {
                passwordError = "Password must contain at least 1 uppercase character";
            }
            else if (!(model.Password.Any(char.IsLower)))
            {
                passwordError = "Password must contain at least 1 lowercase character";
            }
            else if (!(model.Password.Any(char.IsDigit)))
            {
                passwordError = "Password must contain at least 1 number";
            }
            else if ((regexItem.IsMatch(model.Password)))
            {
                passwordError = "Password must contain at least 1 special character";
            }

            if (passwordError.Length > 0)
            {
                return new UserManagerResponse
                {
                    Message = passwordError,
                    IsSuccess = false
                };
            }

            var result = await _userManager.CreateAsync(ApplicationUser, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(ApplicationUser, "User");

                //Generate email confirmation token
                var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(ApplicationUser);
                //Generate byte array for the token
                var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                string url = $"{_configuration["AppUrl"]}/api/auth/confirmemail?userId={ApplicationUser.Id}&token={validEmailToken}";

                //Send email confirmation
                await _mailService.SendEmailAsync(model.Email, "Confirm your email", "<h1>Welcome to SMS Gateway</h1>" +
                    $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");

                return new UserManagerResponse
                {
                    Message = "User created successfully",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                Message = "User failed to be created",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "Email or password is invalid",
                    IsSuccess = false
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Email or password is invalid",
                    IsSuccess = false
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Key));
            var expireDate = DateTime.Now.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                claims: claims,
                expires: expireDate,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpiryDate = expireDate
            };
        }

        public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "User not found",
                    IsSuccess = false
                };
            }

            /*var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);*/

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "Email confirmed successfully",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                Message = "Email cannot be confirmed",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };

        }

        public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "No user associated with email",
                    IsSuccess = false
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            /*var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);*/

            string url = $"{_configuration["AppUrl"]}/ResetPassword?email={email}&token={token}";

            await _mailService.SendEmailAsync(email, "Reset Password",
                                                "<h1> Follow the instruction to reset your password </h1>" +
                                                $"To reset your password, click here : {url}");

            return new UserManagerResponse
            {
                Message = "Reset password URL has been sent to your email successfully",
                IsSuccess = true
            };
        }

        public async Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "No user associated with email",
                    IsSuccess = false
                };
            }

            /*var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);*/

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "Password has been reset succcessfully",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                Message = "Failed to reset password",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }
    }
}