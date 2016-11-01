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

namespace FindogMobile
{
    [Activity(Label = "FoundDog")]
    public class FoundDog : Activity
    {
        ListView wantedDogsListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FoundDogs);

            var result = FetchAnimalsAsync();

            wantedDogsListView = FindViewById<ListView>(Resource.Id.FoundDogList);
            wantedDogsListView.Adapter = new DogAdapter(this, result);
            wantedDogsListView.ItemClick += (s, e) =>
            {
                var listView = s as ListView;
                var t = result[e.Position];
                Toast.MakeText(this, t.Breed, ToastLength.Short).Show();
                Intent intent = new Intent(this, typeof(DogDescription));
                Bundle bundle = new Bundle();
                bundle.PutString("breed", t.Breed);
                bundle.PutDouble("latitude", t.Latitude);
                bundle.PutString("description", t.Description);
                bundle.PutByteArray("image", t.Image);
                bundle.PutDouble("longitude", t.Longitude);
                bundle.PutDouble("latitude", t.Latitude);
                intent.PutExtra("bundle", bundle);
                StartActivity(intent);
            };
        }

        private List<Dog> FetchAnimalsAsync()
        {
            List<Dog> dogs = new List<Dog>();
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
                    Dog dog = new Dog();
                    dog.Breed = animal["breed"].ToString();
                    dog.Description = animal["description"].ToString();
                    dog.Date = animal["date"].ToObject<DateTime>();
                    dog.Image = animal["image"].ToObject<byte[]>();
                    dog.Latitude = animal["latitude"].ToObject<int>();
                    dog.Longitude = animal["longitude"].ToObject<int>();
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