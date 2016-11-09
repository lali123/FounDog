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
using BusinessLogic.Models;
using FindogMobile.Fragments;
using Java.Lang;

namespace FindogMobile.Adapters
{
    class FIndogTabsUpdateAdapter : Android.Support.V4.App.FragmentStatePagerAdapter
    {
        const int PAGE_COUNT = 3;
        private string[] tabTitles = { "Camera", "Details", "Map" };
        readonly Context context;
        Animal dog;

        public FIndogTabsUpdateAdapter(Context context, Android.Support.V4.App.FragmentManager fm, Animal dog) : base(fm)
        {
            this.context = context;
            this.dog = dog;
        }

        public override int Count
        {
            get { return PAGE_COUNT; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            if (position == 0)
            {
                return CameraUpdateFragment.NewInstance(position + 1, dog);
            }
            else if (position == 1)
            {
                return FindogDetailsUpdateFragment.NewInstance(position + 1, dog); ;
            }
            else
            {
                return MapFragmentUpdate.NewInstance(position + 1, dog);
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