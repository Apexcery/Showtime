using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace Showtime.Mob.Fragments
{
    public class UserFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_user, container, false);
        }
    }
}