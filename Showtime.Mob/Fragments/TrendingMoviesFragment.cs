using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using Showtime.Mob.Adapters;
using Showtime.Mob.Interfaces;
using Showtime.Mob.Models;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Showtime.Mob.Fragments
{
    public class TrendingMoviesFragment : Fragment, SwipeRefreshLayout.IOnRefreshListener
    {
        private readonly ITmdbApi _tmdbApi;

        public TrendingMoviesFragment(ITmdbApi tmdbApi)
        {
            _tmdbApi = tmdbApi;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragmentView = inflater.Inflate(Resource.Layout.fragment_trending_movies, container, false);

            PopulateTrendingMovies(fragmentView);

            var refreshLayout = fragmentView.FindViewById<SwipeRefreshLayout>(Resource.Id.trending_movies_srl);
            refreshLayout.SetOnRefreshListener(this);

            return fragmentView;
        }

        private async void PopulateTrendingMovies(View fragmentView)
        {
            var movies = await _tmdbApi.GetTrendingMovies();
            if (movies == null)
                Toast.MakeText(Application.Context, "Trending movies could not be found, check your network connection.", ToastLength.Long).Show();

            var trendingMovies = movies.Select(x => new TrendingMovieDetails
            {
                Title = x.Title,
                ImageUrl = x.PosterPath
            }).Take(15).ToList();

            var recycler = fragmentView.FindViewById<RecyclerView>(Resource.Id.trending_movies_recycler);
            var trendingMoviesAdapter = new TrendingMoviesAdapter(Activity, trendingMovies);
            recycler.SetAdapter(trendingMoviesAdapter);
            RecyclerView.LayoutManager layoutManager = new GridLayoutManager(Activity, 3);
            recycler.SetLayoutManager(layoutManager);
            recycler.SetItemAnimator(new DefaultItemAnimator());
        }

        public void OnRefresh()
        {
            Activity.SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.main_content, new TrendingFragment(_tmdbApi))
                .DisallowAddToBackStack()
                .Commit();
        }
    }
}