using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.BottomNavigation;

namespace Showtime.Mob
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
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
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            item.SetChecked(true);

            switch (item.ItemId)
            {
                case Resource.Id.action_trending:
                    return true;
                case Resource.Id.action_user:
                    return true;
            }

            return false;
        }
    }
}