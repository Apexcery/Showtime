using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Showtime.Lib.Models;
using Showtime.Lib.Models.Auth;
using Showtime.Web.Enums.Tmdb;
using Showtime.Web.Models;
using Showtime.Web.Services;
using Showtime.Web.ViewModels;

namespace Showtime.Web.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITmdbService _tmdbService;

        public HomeController(IAuthService authService, ITmdbService tmdbService)
        {
            _authService = authService;
            _tmdbService = tmdbService;
        }

        public async Task<IActionResult> Index()
        {
            var loggedIn = Request.Cookies.TryGetValue("access-token", out var token);
            var headers = Response.Headers.Values;
            // var loggedIn = HttpContext.Session.TryGetValue("access-token", out var token);
            if (!loggedIn)
                return RedirectToAction("Login", new LoginViewModel());

            var trendingMovies = await _tmdbService.GetTrendingMovies(TimeWindow.Day);
            var trendingTv = await _tmdbService.GetTrendingTv(TimeWindow.Day);

            var viewModel = new HomeViewModel
            {
                Movies = trendingMovies,
                Tv = trendingTv
            };

            return View(viewModel);
        }

        [Route("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            Response.Cookies.Delete("access-token");
            return RedirectToAction("Index");
        }

        [Route("Login")]
        public IActionResult Login()
        {
            var loggedIn = Request.Cookies.TryGetValue("access-token", out var token);
            // var loggedIn = HttpContext.Session.TryGetValue("access-token", out var token);
            if (loggedIn)
                return RedirectToAction("Index");

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

                var cookieOptions = new CookieOptions { Expires = loginResponse.Expiration, HttpOnly = true };
                Response.Cookies.Append("access-token", loginResponse.AccessToken, cookieOptions);
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
                    Response.Cookies.Append("access-token", loginResponseModel.AccessToken, cookieOptions);
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

        [Route("Search")]
        public async Task<IActionResult> Search(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
                return RedirectToAction("Index");

            var searchResults = await _tmdbService.Search(searchQuery);
            
            var viewModel = new SearchViewModel
            {
                SearchQuery = searchQuery,
                SearchResults = searchResults
            };

            return View(viewModel);
        }

        [Route("Error/{code:int?}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? code, string errorMessage)
        {
            return View(new ErrorViewModel {StatusCode = code, ErrorMessage = errorMessage});
        }
    }
}
