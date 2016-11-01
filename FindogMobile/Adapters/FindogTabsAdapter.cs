using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FindogMobile.Fragments;
using Java.Lang;
using Android.Content.Res;
using FindogMobile.Helpers;

namespace FindogMobile.Adapters
{
    class FindogTabsAdapter : Android.Support.V4.App.FragmentStatePagerAdapter
    {

        const int PAGE_COUNT = 3;
        private string[] tabTitles = { "Camera", "Details", "Map" };
        readonly Context context;

        public FindogTabsAdapter(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            this.context = context;
        }

        public override int Count
        {
            get { return PAGE_COUNT; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            if (position == 0)
            {
                return CameraFragment.NewInstance(position + 1);
            }
            else if (position == 1)
            {
                return FindogDetailsFragment.NewInstance(position + 1); ;
            }
            else
            {
                return MapFragment.NewInstance(position + 1);
            }
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            // Generate title based on item position
            return CharSequence.ArrayFromStringArray(tabTitles)[position];
        }

        public View GetTabView(int position)
        {
            // Given you have a custom layout in `res/layout/custom_tab.xml` with a TextView
            var tv = (TextView)LayoutInflater.From(context).Inflate(Resource.Layout.TabHeader, null);
            tv.Text = tabTitles[position];
            return tv;
        }

        public override IParcelable SaveState()
        {
            return base.SaveState();
        }
    }
}