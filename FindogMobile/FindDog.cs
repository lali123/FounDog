using Android.App;
using Android.OS;
using Android.Views;
using FindogMobile.Adapters;
using FindogMobile.Fragments;
using FindogMobile.Helpers;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views.InputMethods;
using Android.Content;
using System;

namespace FindogMobile
{
    [Activity(Label = "FindDog")]
    public class FindDog : AppCompatActivity
    {
        private TabLayout tabLayout;
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

            var adapter = new FindogTabsAdapter(this, SupportFragmentManager);
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
                    if (f is FindogDetailsFragment)
                    {
                        (f as FindogDetailsFragment).RefreshImage();
                        (f as FindogDetailsFragment).longitudeTextView.Text = Longitude.ToString();
                        (f as FindogDetailsFragment).latitudeTextView.Text = Latitude.ToString();
                        InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
                        var currentFocus = (f as FindogDetailsFragment).breedEditView;
                        if (currentFocus != null)
                        {
                            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
                        }
                        //break;
                    }
                    else if (f is CameraFragment)
                    {
                        InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
                        var currentFocus = this.CurrentFocus;
                        if (currentFocus != null)
                        {
                            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
                        }
                    }
                    else if (f is MapFragment)
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