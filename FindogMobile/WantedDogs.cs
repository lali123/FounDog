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
using FindogMobile.Adapters;
using FindogMobile.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using BusinessLogic.Models;

namespace FindogMobile
{
    [Activity(Label = "WantedDogs")]
    public class WantedDogs : Activity
    {
        DogAdapter adapter;
        Android.Support.V7.Widget.SearchView searchView;
        ListView wantedDogsListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WantedDogs);

            var result = FetchAnimalsAsync();
            adapter = new DogAdapter(this, result);
            wantedDogsListView = FindViewById<ListView>(Resource.Id.WantedDogsList);
            wantedDogsListView.Adapter = adapter;
            wantedDogsListView.ItemClick += (s, e) =>
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


            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dogwantedtoolbar);

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

        private List<Animal> FetchAnimalsAsync()
        {
            List<Animal> dogs = new List<Animal>();
            try
            {
                string responseFromServer = String.Empty;
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"animal/wantedanimals");
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