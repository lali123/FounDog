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

namespace FindogMobile
{
    [Activity(Label = "FindDog")]
    public class FindDog : AppCompatActivity
    {
        private TabLayout tabLayout;
        private ViewPager findogViewPager;
        private Toolbar toolbar;

        public string Breed { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FindDog);
            var adapter = new FindogTabsAdapter(this, SupportFragmentManager);
            findogViewPager = FindViewById<ViewPager>(Resource.Id.pager);
            findogViewPager.Adapter = adapter;
            findogViewPager.CurrentItem = 1;
            findogViewPager.OffscreenPageLimit = 3;
            findogViewPager.PageSelected += (s, e) =>
            {
                if (e.Position == 1)
                {
                    foreach (var f in SupportFragmentManager.Fragments)
                    {
                        //if (f is FindogDetailsFragment)
                        //{
                        //    //(f as FindogDetailsFragment).RefreshImage();
                        //    break;
                        //}
                    }
                }
            };
            toolbar = FindViewById<Toolbar>(Resource.Id.my_toolbar);
            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);

            SetSupportActionBar(toolbar);

            // Setup tablayout with view pager
            tabLayout.SetupWithViewPager(findogViewPager);
            for (int i = 0; i < tabLayout.TabCount; i++)
            {
                TabLayout.Tab tab = tabLayout.GetTabAt(i);
                tab.SetCustomView(adapter.GetTabView(i));
            }

            Breed = "";
            Description = "";
            Latitude = 0;
            Longitude = 0;
        }
    }
}