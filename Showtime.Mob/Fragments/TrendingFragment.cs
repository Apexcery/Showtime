using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.Tabs;
using Showtime.Mob.Adapters;
using Showtime.Mob.Interfaces;

namespace Showtime.Mob.Fragments
{
    public class TrendingFragment : Fragment
    {
        private readonly ITmdbApi _tmdbApi;

        public TrendingFragment(ITmdbApi tmdbApi)
        {
            _tmdbApi = tmdbApi;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragmentView = inflater.Inflate(Resource.Layout.fragment_trending, container, false);

            var viewPager = fragmentView.FindViewById<ViewPager>(Resource.Id.trending_view_pager);
            var tabLayout = fragmentView.FindViewById<TabLayout>(Resource.Id.trending_tab_layout);

            var adapter = new TabAdapter(Activity.SupportFragmentManager);
            adapter.AddItem(new TrendingMoviesFragment(_tmdbApi), "Movies");
            adapter.AddItem(new TrendingTvFragment(), "TV Shows");

            viewPager.Adapter = adapter;
            tabLayout.SetupWithViewPager(viewPager);
            return fragmentView;
        }
    }
}