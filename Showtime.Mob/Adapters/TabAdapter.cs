using System.Collections.Generic;
using AndroidX.Fragment.App;

namespace Showtime.Mob.Adapters
{
    public class TabAdapter : FragmentStatePagerAdapter
    {
        private readonly List<Fragment> _fragmentList = new List<Fragment>();
        private readonly List<string> _fragmentTitleList = new List<string>();


        public TabAdapter(FragmentManager fm) : base(fm) { }

        public override Fragment GetItem(int position)
        {
            return _fragmentList[position];
        }

        public void AddItem(Fragment fragment, string title)
        {
            _fragmentList.Add(fragment);
            _fragmentTitleList.Add(title);
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(_fragmentTitleList[position]);
        }

        public override int Count => _fragmentList.Count;
    }
}
