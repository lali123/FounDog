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
using Android.Graphics;
using FindogMobile.Models;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Support.V7.App;
using FindogMobile.Adapters;
using Android.Support.V4.View;
using Android.Support.Design.Widget;

namespace FindogMobile
{
    [Activity(Label = "DogDescription")]
    public class DogDescription : AppCompatActivity
    {
        private TabLayout tabLayout;
        Animal animal;
        private ViewPager findogViewPager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DogDescription);

            Intent intent = Intent;
            Bundle bundle = intent.Extras.GetBundle("bundle");
            animal = new Animal();
            animal.UserId = bundle.GetString("userId");
            animal.Image = bundle.GetByteArray("image");
            animal.Breed = bundle.GetString("breed");
            animal.Description = bundle.GetString("description");
            animal.Latitude = bundle.GetDouble("latitude");
            animal.Longitude = bundle.GetDouble("longitude");


            var adapter = new FindogTabsReadOnlyAdapter(this, SupportFragmentManager, animal);
            findogViewPager = FindViewById<ViewPager>(Resource.Id.pagerReadOnly);
            findogViewPager.Adapter = adapter;
            findogViewPager.CurrentItem = 1;
            findogViewPager.OffscreenPageLimit = 3;
            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabsReadOnly);
            tabLayout.SetupWithViewPager(findogViewPager);
            for (int i = 0; i < tabLayout.TabCount; i++)
            {
                TabLayout.Tab tab = tabLayout.GetTabAt(i);
                tab.SetCustomView(adapter.GetTabView(i));
            }
        }
    }
}