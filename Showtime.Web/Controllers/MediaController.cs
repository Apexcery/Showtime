using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Showtime.Web.Services;

namespace Showtime.Web.Controllers
{
    [Route("")]
    public class MediaController : Controller
    {
        private readonly ITmdbService _tmdbService;

        public MediaController(ITmdbService tmdbService)
        {
            _tmdbService = tmdbService;
        }

        [Route("Movie/{movieId}")]
        public async Task<IActionResult> Movie(int? movieId)
        {
            if (movieId == null)
                return RedirectToAction("Index", "Home");

            var movieDetails = await _tmdbService.GetFullMovieDetails((int) movieId);
            if (movieDetails == null)
                return RedirectToAction("Index", "Home");

            return View(movieDetails);
        }
    }
}
