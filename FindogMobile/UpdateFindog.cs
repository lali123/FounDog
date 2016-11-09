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
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using FindogMobile.Adapters;
using BusinessLogic.Models;
using FindogMobile.Fragments;
using Android.Views.InputMethods;
using Newtonsoft.Json;

namespace FindogMobile
{
    [Activity(Label = "UpdateFindog")]
    public class UpdateFindog : AppCompatActivity
    {
        private TabLayout tabLayout;
        Animal animal;
        private ViewPager findogViewPager;
        //private Toolbar toolbar;

        public string Breed { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FindDog);

            Latitude = 47.5316049;
            Longitude = 21.6273123;
            Breed = "";
            Description = "";

            Intent intent = Intent;
            Bundle bundle = intent.Extras.GetBundle("bundle");
            
            animal = new Animal();
            animal.AnimalIdToObjectId(bundle.GetString("AnimalId"));
            animal.UserId = bundle.GetString("userId");
            animal.Image = bundle.GetByteArray("image");
            animal.Breed = bundle.GetString("breed");
            animal.Description = bundle.GetString("description");
            animal.Latitude = bundle.GetDouble("latitude");
            animal.Longitude = bundle.GetDouble("longitude");


            var adapter = new FIndogTabsUpdateAdapter(this, SupportFragmentManager, animal);
            findogViewPager = FindViewById<ViewPager>(Resource.Id.pager);
            findogViewPager.Adapter = adapter;
            findogViewPager.CurrentItem = 1;
            findogViewPager.OffscreenPageLimit = 3;
            findogViewPager.PageSelected += PageSelected;

            //toolbar = FindViewById<Toolbar>(Resource.Id.my_toolbar);
            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);

            // Setup tablayout with view pager
            tabLayout.SetupWithViewPager(findogViewPager);
            for (int i = 0; i < tabLayout.TabCount; i++)
            {
                TabLayout.Tab tab = tabLayout.GetTabAt(i);
                tab.SetCustomView(adapter.GetTabView(i));
            }
        }

        private void PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if (e.Position == 1)
            {
                foreach (var f in SupportFragmentManager.Fragments)
                {
                    if (f is FindogDetailsUpdateFragment)
                    {
                        (f as FindogDetailsUpdateFragment).RefreshImage();
                        (f as FindogDetailsUpdateFragment).longitudeTextView.Text = Longitude.ToString();
                        (f as FindogDetailsUpdateFragment).latitudeTextView.Text = Latitude.ToString();
                        InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
                        var currentFocus = (f as FindogDetailsUpdateFragment).breedEditView;
                        if (currentFocus != null)
                        {
                            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
                        }
                    }
                    else if (f is CameraUpdateFragment)
                    {
                        InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
                        var currentFocus = this.CurrentFocus;
                        if (currentFocus != null)
                        {
                            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
                        }
                    }
                    else if (f is MapFragmentUpdate)
                    {
                        InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
                        var currentFocus = this.CurrentFocus;
                        if (currentFocus != null)
                        {
                            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
                        }
                    }
                }
            }
        }
    }
}