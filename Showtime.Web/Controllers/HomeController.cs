using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Showtime.Lib.Models;
using Showtime.Lib.Models.Auth;
using Showtime.Web.Models;
using Showtime.Web.Services;

namespace Showtime.Web.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public HomeController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        public IActionResult Index()
        {
            var loggedIn = Request.Cookies.TryGetValue("access-token", out var token);
            // var loggedIn = HttpContext.Session.TryGetValue("access-token", out var token);
            if (!loggedIn)
                return RedirectToAction("Login", new LoginViewModel());

            return View();
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            model.UsernameOrEmail ??= string.Empty;
            model.Password ??= string.Empty;

            var response = await _authService.Login(model);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse == null)
                    return BadRequest();

                var cookieOptions = new CookieOptions { Expires = loginResponse.Expiration };
                Response.Cookies.Append("access-token", loginResponse.Token, cookieOptions);
                // HttpContext.Session.SetString("access-token", loginResponse.Token);
                return RedirectToAction("Index");
            }

            var responseString = await response.Content.ReadAsStringAsync();

            var loginViewModel = new LoginViewModel();

            var error = JsonConvert.DeserializeObject<Error>(responseString);
            if (error.StatusCode == 0 && string.IsNullOrEmpty(error.ErrorMessage))
            {
                var validationError = JsonConvert.DeserializeObject<LoginValidationError>(responseString);
                var validationErrorList = new List<string>();
                if (validationError.Errors.UsernameOrEmail != null)
                    validationErrorList.AddRange(validationError.Errors.UsernameOrEmail);
                if (validationError.Errors.Password != null)
                    validationErrorList.AddRange(validationError.Errors.Password);
                foreach (var errorMessage in validationErrorList)
                {
                    loginViewModel.ErrorMessages.Add(errorMessage);
                }
            }
            else
                loginViewModel.ErrorMessages.Add(error.ErrorMessage);


            return View(loginViewModel);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View("Login", new LoginViewModel
                {
                    ErrorMessages =
                    {
                        "Register details were not valid."
                    }
                });

            var response = await _authService.Register(model);
            if (response.IsSuccessStatusCode)
            {
                var registerResponse = await response.Content.ReadFromJsonAsync<RegisterResponse>();
                if (registerResponse == null)
                    return BadRequest();

                var loginModel = new LoginModel
                {
                    UsernameOrEmail = model.Username,
                    Password = model.Password
                };

                var loginResponse = await _authService.Login(loginModel);
                if (loginResponse.IsSuccessStatusCode)
                {
                    var loginResponseModel = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
                    if (loginResponseModel == null)
                        return BadRequest();

                    var cookieOptions = new CookieOptions{ Expires = loginResponseModel.Expiration};
                    Response.Cookies.Append("access-token", loginResponseModel.Token, cookieOptions);
                    // HttpContext.Session.SetString("access-token", loginResponseModel.Token);
                    return RedirectToAction("Index");
                }
            }

            var responseString = await response.Content.ReadAsStringAsync();

            var loginViewModel = new LoginViewModel();

            var error = JsonConvert.DeserializeObject<Error>(responseString);
            if (error.StatusCode == 0 && string.IsNullOrEmpty(error.ErrorMessage))
            {
                var validationError = JsonConvert.DeserializeObject<LoginValidationError>(responseString);
                var validationErrorList = new List<string>();
                if (validationError.Errors.UsernameOrEmail != null)
                    validationErrorList.AddRange(validationError.Errors.UsernameOrEmail);
                if (validationError.Errors.Password != null)
                    validationErrorList.AddRange(validationError.Errors.Password);
                foreach (var errorMessage in validationErrorList)
                {
                    loginViewModel.ErrorMessages.Add(errorMessage);
                }
            }
            else
                loginViewModel.ErrorMessages.Add(error.ErrorMessage);

            return View("Login", loginViewModel);
        }

        [Route("Error/{code:int?}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? code, string errorMessage)
        {
            return View(new ErrorViewModel {StatusCode = code, ErrorMessage = errorMessage});
        }
    }
}
