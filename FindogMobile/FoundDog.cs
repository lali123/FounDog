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
using FindogMobile.Models;
using FindogMobile.Adapters;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BusinessLogic.Models;

namespace FindogMobile
{
    [Activity(Label = "FoundDog")]
    public class FoundDog : Android.Support.V7.App.AppCompatActivity
    {
        ListView foundDogsListView;
        DogAdapter adapter;
        Android.Support.V7.Widget.SearchView searchView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FoundDogs);
                       
            var result = FetchAnimalsAsync();
            adapter = new DogAdapter(this, result);
            foundDogsListView = FindViewById<ListView>(Resource.Id.FoundDogList);
            foundDogsListView.Adapter = adapter;
            foundDogsListView.ItemClick += (s, e) =>
            {
                var listView = s as ListView;
                var t = result[e.Position];
                Toast.MakeText(this, t.Breed, ToastLength.Short).Show();
                Intent intent = new Intent(this, typeof(DogDescription));
                Bundle bundle = new Bundle();
                bundle.PutString("userId", t.UserId.ToString());
                bundle.PutString("breed", t.Breed);
                bundle.PutDouble("latitude", t.Latitude);
                bundle.PutString("description", t.Description);
                bundle.PutByteArray("image", t.Image);
                bundle.PutDouble("longitude", t.Longitude);
                bundle.PutDouble("latitude", t.Latitude);
                intent.PutExtra("bundle", bundle);
                StartActivity(intent);
            };


            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dogtoolbar);

            toolbar.InflateMenu(Resource.Menu.ItemSearch);
            toolbar.MenuItemClick += (object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e) =>
            {

            };

            var search = toolbar.Menu.FindItem(Resource.Id.search);
            searchView = search.ActionView.JavaCast<Android.Support.V7.Widget.SearchView>();
            searchView.QueryHint = "Search";
            searchView.QueryTextChange += (s, e) => { adapter.Filter(e.NewText); };
            searchView.QueryTextSubmit += (s, e) =>
            {
                Toast.MakeText(this, "Searched for: " + e.Query, ToastLength.Short).Show();
                e.Handled = true;
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ItemSearch, menu);

            var item = menu.FindItem(Resource.Id.dogtoolbar);

            var searchView = Android.Support.V4.View.MenuItemCompat.GetActionView(item);
            var _searchView = searchView.JavaCast<SearchView>();

            _searchView.QueryTextChange += (s, e) => adapter.Filter(e.NewText);

            _searchView.QueryTextSubmit += (s, e) =>
            {
                // Handle enter/search button on keyboard here
                Toast.MakeText(this, "Searched for: " + e.Query, ToastLength.Short).Show();
                e.Handled = true;
            };

            // Android.Support.V4.View.MenuItemCompat.SetOnActionExpandListener(item, new SearchViewExpandListener(adapter));

            return true;
        }

        //private class SearchViewExpandListener
        //    : Java.Lang.Object, Android.Support.V4.View.MenuItemCompat.IOnActionExpandListener
        //{
        //    private readonly IFilterable _adapter;

        //    public SearchViewExpandListener(IFilterable adapter)
        //    {
        //        _adapter = adapter;
        //    }

        //    public bool OnMenuItemActionCollapse(IMenuItem item)
        //    {
        //        _adapter.Filter.InvokeFilter("");
        //        return true;
        //    }

        //    public bool OnMenuItemActionExpand(IMenuItem item)
        //    {
        //        return true;
        //    }
        //}

        private List<Animal> FetchAnimalsAsync()
        {
            List<Animal> dogs = new List<Animal>();
            try
            {
                string responseFromServer = String.Empty;
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"animal/foundanimals");
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Get the stream containing content returned by the server.
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Read the content.
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            responseFromServer = reader.ReadToEnd();
                        }
                    }
                }

                JArray animals = JArray.Parse(responseFromServer);

                foreach (var animal in animals)
                {
                    Animal dog = new Animal();
                    dog.UserId = animal["userId"].ToString();
                    dog.Breed = animal["breed"].ToString();
                    dog.Description = animal["description"].ToString();
                    dog.Date = animal["date"].ToObject<DateTime>();
                    dog.Image = animal["image"].ToObject<byte[]>();
                    dog.Latitude = animal["latitude"].ToObject<double>();
                    dog.Longitude = animal["longitude"].ToObject<double>();
                    dogs.Add(dog);
                }
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Cannot connect to the server", ToastLength.Short).Show();
            }


            return dogs;
        }
    }
}