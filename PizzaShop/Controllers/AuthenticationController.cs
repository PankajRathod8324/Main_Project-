using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL.ViewModel;
using BLL.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IdentityModel.Tokens.Jwt;
using DAL.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

namespace PizzaShop.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;


        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("Loginpage")]
        public IActionResult Loginpage()
        {
            var user = new LoginVM();
            if (Request.Cookies["Email"] != null && Request.Cookies["Password"] != null)
            {
                user.Email = Request.Cookies["Email"];
                user.Password = Request.Cookies["Password"];
            }
            return View(user);
        }
        [HttpPost("Loginpage")]
        public async Task<IActionResult> Loginpage(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = _userService.AuthenticateUser(model.Email, model.Password);// chg to string (remove var)
            if (token == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            if (model.RememberMe)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30)
                };
                Response.Cookies.Append("Email", model.Email, cookieOptions);
                Response.Cookies.Append("AuthToken", token);

            }
            else
            {
                Response.Cookies.Delete("Email");
            }
            // Store JWT in Session
            HttpContext.Session.SetString("AuthToken", token);
            var email = model.Email;
            var user = _userService.GetUserByEmail(email);
            HttpContext.Session.SetString("UserData", JsonConvert.SerializeObject(user));

            var sessionData = HttpContext.Session.GetString("UserData");
            Console.WriteLine(sessionData);

            // Extract claims from JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var claims = jwtToken?.Claims.ToList();
            Console.WriteLine("Claims:");
            foreach (var claim in claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            var ClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(ClaimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            // Return the token to the client
            return RedirectToAction("Dashboard", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("UserData");
            HttpContext.Session.Clear();
            return RedirectToAction("Loginpage");
        }
        public IActionResult Forgotpasswordpage()
        {
            var user = new ForgotPasswordVM();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            var user = await _userService.GenerateResetTokenAsync(model.Email);
            if (user != null)
            {
                ViewBag.Message = "A reset link has been sent to your email.";
                await SendResetEmail(user.Email, user.ResetToken);
            }

            return View("Forgotpasswordpage");
        }


        // [HttpGet("ResetPassword")]
        public IActionResult Resetpasswordpage(string email, string token)
        {

            var user = _userService.GetUserByToken(token);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ResetPasswordVM
            {
                Email = email,
                Token = token
            };
            Console.WriteLine(model.Token);
            Console.WriteLine("Auth Controller:  " + model.Email);
            Console.WriteLine(model.Token);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            Console.WriteLine("In resetPage");
            Console.WriteLine(model.Email);
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is not valid.");
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                Console.WriteLine("Passwords do not match.");
                return View(model);
            }
            Console.WriteLine(model.Email);
            var result = await _userService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);
            Console.WriteLine("EMAIL:" + model.Email);

            if (result)
            {
                ViewBag.Message = "A Password Reset Successful.";
                Console.WriteLine("Password reset process successful.");
                return RedirectToAction("Loginpage", "Authentication");
            }

            ModelState.AddModelError("", "Invalid token or the token has expired.");
            Console.WriteLine("Invalid token or the token has expired.");
            return View("Resetpasswordpage", model);
        }
        private async Task SendResetEmail(string email, string token)
        {
            var resetLink = Url.Action("Resetpasswordpage", "Authentication", new { token, email }, Request.Scheme);
            var message = new MailMessage("test.dotnet@etatvasoft.com", email);
            message.To.Add(new MailAddress(email));
            message.Subject = "Password Reset Request";
            message.Body = $"Please reset your password by clicking here: <a href='{resetLink}'>link</a>";
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                smtp.Host = "mail.etatvasoft.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("test.dotnet@etatvasoft.com", "P}N^{z-]7Ilp");
                await smtp.SendMailAsync(message);
            }
        }
    }
}
