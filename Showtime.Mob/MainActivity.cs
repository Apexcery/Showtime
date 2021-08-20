using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.BottomNavigation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Showtime.Mob.Enums;
using Showtime.Mob.Fragments;
using Showtime.Mob.Interfaces;
using Showtime.Mob.Models;
using Showtime.Mob.Services;
using Environment = System.Environment;

namespace Showtime.Mob
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        protected static IServiceProvider ServiceProvider { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetupServices();

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            if (FindViewById(Resource.Id.main_toolbar) is Toolbar toolbar)
            {
                toolbar.SetTitle(Resource.String.trending_title);
                SetSupportActionBar(toolbar);
            }

            var bottomNavView = FindViewById<BottomNavigationView>(Resource.Id.main_bottom_nav);
            bottomNavView?.SetOnNavigationItemSelectedListener(this);

            TryChangeMainFragment(FragmentEnum.Trending);
        }

        private void SetupServices()
        {
            var services = new ServiceCollection();

            var appSettingsString = "";
            using (StreamReader sr = new StreamReader(Assets.Open("appSettings.json")))
            {
                appSettingsString = sr.ReadToEnd();
            }

            var appSettings = JsonConvert.DeserializeObject<Settings>(appSettingsString);
            var tmdbSettings = appSettings.appSettings.ExternalApis.Tmdb;
            services.AddHttpClient<ITmdbApi>(client => GetTmdbClient(tmdbSettings.BaseUrl, tmdbSettings.ApiToken));

            ServiceProvider = services.BuildServiceProvider();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            item.SetChecked(true);

            return item.ItemId switch
            {
                Resource.Id.action_trending => TryChangeMainFragment(FragmentEnum.Trending),
                Resource.Id.action_user => TryChangeMainFragment(FragmentEnum.User),
                _ => false
            };
        }

        public bool TryChangeMainFragment(FragmentEnum fragmentEnum)
        {

            var appSettingsString = "";
            using (StreamReader sr = new StreamReader(Assets.Open("appSettings.json")))
            {
                appSettingsString = sr.ReadToEnd();
            }

            var appSettings = JsonConvert.DeserializeObject<Settings>(appSettingsString);
            var tmdbSettings = appSettings.appSettings.ExternalApis.Tmdb;
            var tmdbApi = new TmdbAPI(GetTmdbClient(tmdbSettings.BaseUrl, tmdbSettings.ApiToken));
            switch (fragmentEnum)
            {
                case FragmentEnum.Trending:
                    SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.main_content, new TrendingFragment(tmdbApi))
                        .SetReorderingAllowed(false)
                        .DisallowAddToBackStack()
                        .Commit();
                    return true;
                case FragmentEnum.User:
                    SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.main_content, new UserFragment())
                        .SetReorderingAllowed(false)
                        .DisallowAddToBackStack()
                        .Commit();
                    return true;
                default:
                    return false;
            }
        }

        private HttpClient GetTmdbClient(string baseUrl, string tmdbToken)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tmdbToken);
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            return httpClient;
        }
    }
}